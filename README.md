# junior-tennis

大阪府テニス協会ジュニアランキングシステム。

## Projects
### JuniorTennis.Domain
業務ロジックライブラリ。
ValueObject、FirstCollectionObject、Entityなどを用いて仕様をコードで表現する。

#### ValueObject
[値オブジェクトを実装する](https://docs.microsoft.com/ja-jp/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects)

不変な値。最小の仕様を表現する。値オブジェクトには主に 2 つの特性がある。
* ID がない。
* 不変である。

IDがないため副作用がなく追跡する必要がない。独立して不変な値である。
たとえば氏名などをstring型にするのではなくFirstName型、FamilyName型といったクラスを作成する。
ValueObjectの中で最大文字数や禁止文字のチェックを行うことでデータに対して仕様が明確になる。

#### Enumeration
[enum 型の代わりに Enumeration クラスを使用する](https://docs.microsoft.com/ja-jp/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types)

列挙型クラス。
列挙型の場合、制御フローや抽象化が外部に漏れるためオブジェクトとすることで仕様を関連付けることができる。

#### Entity
複数の仕様を用いた集約オブジェクト。

#### DDDについて
Domain Driven Design.
[モデルでドメイン知識を表現するとは何か[DDD]](https://little-hands.hatenablog.com/entry/2017/10/04/201201)
[DDD基礎解説：Entity、ValueObjectってなんなんだ](https://little-hands.hatenablog.com/entry/2018/12/09/entity-value-object)
[新卒にも伝わるドメイン駆動設計のアーキテクチャ説明(オニオンアーキテクチャ)[DDD]](https://little-hands.hatenablog.com/entry/2018/12/10/ddd-architecture)

### JuniorTennis.DomainTests
業務ロジックライブラリのテストプロジェクト。

* xUnit
* Moq

### JuniorTennis.Infrastructure
DBなどの外部サービス。

* EFCore

#### Database
DataAccessライブラリ。
EntityFramework Coreを用いてデータの永続化を実現する。

#### Email
Eメール送信ライブラリ。

#### Identity
認証ライブラリ。
Microsoft.AspNetCore.Identityを用いて認証および認可を実現する。

### JuniorTennis.Mvc
Webアプリケーション。

#### Features
機能毎に作成する。
Controller、View（cshtml）、ViewModel、Modelをまとめる。

## Database
### 導入
1. DBにPostgeSQL12をインストール
1. JuniorTennis.Mvc/appsettings.jsonのConnectionStrings.JuniorTennisDbContextに合わせてDB構築

```
$ psql -U postgres

$ ユーザ postgres のパスワード:
#  → インストール時に設定したパスワードを入力する

# CREATE ROLE tennis WITH
#   LOGIN
#   NOSUPERUSER
#   NOCREATEDB
#   NOCREATEROLE
#   INHERIT
#   NOREPLICATION
#   CONNECTION LIMIT -1
#   PASSWORD 'tennis';

# CREATE DATABASE junior_tennis
#   WITH 
#   OWNER = tennis
#   ENCODING = 'UTF8'
#   CONNECTION LIMIT = -1;

# DROP SCHEMA public cascade;
# CREATE SCHEMA public;
# ALTER SCHEMA public owner to tennis;
```

### 更新
1. （必要に応じて）ClearTables.batを実行してテーブルを削除する

Migrationのバージョンが複数に分かれてしまった場合、取りまとめるために一度リセットして作り直す。

```
@echo off

SET FILENAME=update.sql
echo drop schema public cascade; >> %FILENAME%
echo create schema public; >> %FILENAME%
echo alter schema public owner to tennis; >> %FILENAME%

SET PGPASSWORD=tennis
psql -h localhost -p 5432 -U tennis -d junior_tennis -f %FILENAME%
del /Q %FILENAME%

pause
```

2. 以下のコマンドをVisualStudio のパッケージマネージャーコンソール（[表示]＞[その他のウィンドウ]）で実行する
```
Add-Migration CreateDrawTable -Context JuniorTennisDbContext -OutputDir DataBase/Migrations -Project JuniorTennis.Infrastructure -StartUpProject JuniorTennis.Mvc
Update-Database CreateDrawTable -Context JuniorTennisDbContext -Project JuniorTennis.Infrastructure -StartUpProject JuniorTennis.Mvc
```

* 【参考】コマンドプロンプト等を使用して.NET Core CLIのコマンドで行う場合は以下のコマンドとなる（cdでプロジェクトのあるディレクトリに入って行う）
```
dotnet ef migrations add CreateDrawTable -c JuniorTennisDbContext -o DataBase/Migrations -p  JuniorTennis.Infrastructure -s JuniorTennis.Mvc
dotnet ef database update CreateDrawTable -c JuniorTennisDbContext -p JuniorTennis.Infrastructure -s JuniorTennis.Mvc
```

* Migration Nameを明示的に指定する場合。 以前の状態にしたい場合はUpdate-Database任意のMigration Nameを指定する。
```
Add-Migration Init -Context JuniorTennisDbContext -OutputDir DataBase/Migrations -Project JuniorTennis.Infrastructure -StartUpProject JuniorTennis.Mvc 
Update-Database -Migration Init -Context JuniorTennisDbContext -Project JuniorTennis.Infrastructure -StartUpProject JuniorTennis.Mvc 
```

* postgresのportが5432である必要がある 他バージョンのpostgresも入れている場合は注意

### 削除
```
Remove-Migration -Context JuniorTennisDbContext -Project JuniorTennis.Infrastructure -StartUpProject JuniorTennis.Mvc 
```

### スクリプトファイル作成
以下のコマンドをVisualStudio のパッケージマネージャーコンソールで実行することでスクリプトファイルを生成できる
```
Script-Migration
```
VisualStudio で構文エラー扱いの赤線が出る場合があるが無視してよい。このSQLを空のDBに流すことで更新できる
