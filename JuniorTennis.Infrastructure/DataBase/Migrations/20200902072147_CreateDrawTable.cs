using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace JuniorTennis.Infrastructure.DataBase.Migrations
{
    public partial class CreateDrawTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    announce_title = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    announcement_genre = table.Column<int>(nullable: true),
                    registered_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    deleted_date_time = table.Column<DateTime>(nullable: true),
                    attached_file_path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "authorization_links",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    authorization_code = table.Column<string>(nullable: true),
                    unique_key = table.Column<string>(nullable: true),
                    registration_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorization_links", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "draw_settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number_of_blocks = table.Column<int>(nullable: true),
                    number_of_draws = table.Column<int>(nullable: true),
                    number_of_entries = table.Column<int>(nullable: true),
                    number_of_winners = table.Column<int>(nullable: true),
                    tournament_grade = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_draw_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operators",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    email_address = table.Column<string>(nullable: true),
                    login_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(nullable: false),
                    player_code = table.Column<string>(nullable: true),
                    player_family_name = table.Column<string>(nullable: true),
                    player_first_name = table.Column<string>(nullable: true),
                    player_family_name_kana = table.Column<string>(nullable: true),
                    player_first_name_kana = table.Column<string>(nullable: true),
                    player_jpin = table.Column<string>(nullable: true),
                    category = table.Column<int>(nullable: true),
                    gender = table.Column<int>(nullable: true),
                    birth_date = table.Column<DateTime>(nullable: true),
                    telephone_number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reservation_numbers",
                columns: table => new
                {
                    registrated_date = table.Column<DateTime>(nullable: false),
                    serial_number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_numbers", x => new { x.registrated_date, x.serial_number });
                });

            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    registration_from_date = table.Column<DateTime>(nullable: false),
                    team_registration_fee = table.Column<int>(nullable: true),
                    player_registration_fee = table.Column<int>(nullable: true),
                    player_trade_fee = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seasons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_code = table.Column<string>(nullable: true),
                    team_type = table.Column<int>(nullable: true),
                    team_name = table.Column<string>(nullable: true),
                    team_abbreviated_name = table.Column<string>(nullable: true),
                    representative_name = table.Column<string>(nullable: true),
                    representative_email_address = table.Column<string>(nullable: true),
                    telephone_number = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    coach_name = table.Column<string>(nullable: true),
                    coach_email_address = table.Column<string>(nullable: true),
                    team_jpin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tournaments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_name = table.Column<string>(nullable: true),
                    tournament_type = table.Column<int>(nullable: true),
                    registration_year = table.Column<DateTime>(nullable: true),
                    type_of_year = table.Column<int>(nullable: true),
                    aggregation_month = table.Column<DateTime>(nullable: true),
                    holding_period = table.Column<string>(nullable: true),
                    venue = table.Column<string>(nullable: true),
                    method_of_payment = table.Column<int>(nullable: true),
                    entry_fee = table.Column<int>(nullable: true),
                    application_period = table.Column<string>(nullable: true),
                    outline = table.Column<string>(nullable: true),
                    tournament_entry_reception_mail_subject = table.Column<string>(nullable: true),
                    tournament_entry_reception_mail_body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournaments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "draw_tables",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tournament_id = table.Column<int>(nullable: false),
                    tennis_event_id = table.Column<string>(nullable: true),
                    tournament_format = table.Column<int>(nullable: true),
                    eligible_players_type = table.Column<int>(nullable: true),
                    MainDrawSettingsId = table.Column<int>(nullable: true),
                    QualifyingDrawSettingsId = table.Column<int>(nullable: true),
                    edit_status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_draw_tables", x => x.id);
                    table.ForeignKey(
                        name: "FK_draw_tables_draw_settings_MainDrawSettingsId",
                        column: x => x.MainDrawSettingsId,
                        principalTable: "draw_settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_draw_tables_draw_settings_QualifyingDrawSettingsId",
                        column: x => x.QualifyingDrawSettingsId,
                        principalTable: "draw_settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "request_players",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<int>(nullable: false),
                    team_id = table.Column<int>(nullable: false),
                    season_id = table.Column<int>(nullable: false),
                    reservation_number = table.Column<string>(nullable: true),
                    reservation_branch_number = table.Column<int>(nullable: false),
                    category = table.Column<int>(nullable: true),
                    approve_state = table.Column<int>(nullable: true),
                    requested_date_time = table.Column<DateTime>(nullable: false),
                    player_registration_fee = table.Column<int>(nullable: true),
                    approve_date_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_request_players_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_request_players_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_request_players_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "request_teams",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(nullable: false),
                    season_id = table.Column<int>(nullable: false),
                    reservation_number = table.Column<string>(nullable: true),
                    approve_state = table.Column<int>(nullable: true),
                    requested_date_time = table.Column<DateTime>(nullable: false),
                    requested_fee = table.Column<int>(nullable: true),
                    approve_date_time = table.Column<DateTime>(nullable: true),
                    mail_state = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_teams", x => x.id);
                    table.ForeignKey(
                        name: "FK_request_teams_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_request_teams_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "holding_dates",
                columns: table => new
                {
                    tournament_id = table.Column<int>(nullable: false),
                    holding_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holding_dates", x => new { x.tournament_id, x.holding_date });
                    table.ForeignKey(
                        name: "FK_holding_dates_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tennis_events",
                columns: table => new
                {
                    tournament_id = table.Column<int>(nullable: false),
                    category = table.Column<int>(nullable: false),
                    gender = table.Column<int>(nullable: false),
                    format = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tennis_events", x => new { x.tournament_id, x.category, x.gender, x.format });
                    table.ForeignKey(
                        name: "FK_tennis_events_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalTable: "tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    draw_table_id = table.Column<int>(nullable: false),
                    block_number = table.Column<int>(nullable: true),
                    participation_classification = table.Column<int>(nullable: true),
                    game_date = table.Column<DateTime>(nullable: true),
                    draw_settings_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocks", x => x.id);
                    table.ForeignKey(
                        name: "FK_blocks_draw_settings_draw_settings_id",
                        column: x => x.draw_settings_id,
                        principalTable: "draw_settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blocks_draw_tables_draw_table_id",
                        column: x => x.draw_table_id,
                        principalTable: "draw_tables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entry_details",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_number = table.Column<int>(nullable: true),
                    participation_classification = table.Column<int>(nullable: true),
                    seed_number = table.Column<int>(nullable: true),
                    can_participation_dates = table.Column<string>(nullable: true),
                    receipt_status = table.Column<int>(nullable: true),
                    from_qualifying = table.Column<bool>(nullable: false),
                    block_number = table.Column<int>(nullable: true),
                    draw_table_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_entry_details_draw_tables_draw_table_id",
                        column: x => x.draw_table_id,
                        principalTable: "draw_tables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_number = table.Column<int>(nullable: true),
                    round_number = table.Column<int>(nullable: true),
                    draw_settings_id = table.Column<int>(nullable: false),
                    block_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_games_blocks_block_id",
                        column: x => x.block_id,
                        principalTable: "blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_games_draw_settings_draw_settings_id",
                        column: x => x.draw_settings_id,
                        principalTable: "draw_settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entry_players",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_code = table.Column<string>(nullable: true),
                    team_name = table.Column<string>(nullable: true),
                    team_abbreviated_name = table.Column<string>(nullable: true),
                    player_code = table.Column<string>(nullable: true),
                    player_family_name = table.Column<string>(nullable: true),
                    player_first_name = table.Column<string>(nullable: true),
                    point = table.Column<int>(nullable: true),
                    entry_detail_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_players", x => x.id);
                    table.ForeignKey(
                        name: "FK_entry_players_entry_details_entry_detail_id",
                        column: x => x.entry_detail_id,
                        principalTable: "entry_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_results",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_status = table.Column<int>(nullable: true),
                    player_classification_of_winner = table.Column<int>(nullable: true),
                    entry_number_of_winner = table.Column<int>(nullable: true),
                    game_score = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_results", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_results_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opponents",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    block_number = table.Column<int>(nullable: true),
                    game_number = table.Column<int>(nullable: true),
                    draw_number = table.Column<int>(nullable: true),
                    seed_level = table.Column<int>(nullable: true),
                    assign_order = table.Column<int>(nullable: true),
                    frame_player_classification = table.Column<int>(nullable: true),
                    is_manually_setting_frame = table.Column<bool>(nullable: false),
                    is_manually_assigned = table.Column<bool>(nullable: false),
                    player_classification = table.Column<int>(nullable: true),
                    entry_number = table.Column<int>(nullable: true),
                    seed_number = table.Column<int>(nullable: true),
                    team_codes = table.Column<string>(nullable: true),
                    team_abbreviated_names = table.Column<string>(nullable: true),
                    player_codes = table.Column<string>(nullable: true),
                    player_names = table.Column<string>(nullable: true),
                    from_game_number = table.Column<int>(nullable: true),
                    game_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opponents", x => x.id);
                    table.ForeignKey(
                        name: "FK_opponents_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blocks_draw_settings_id",
                table: "blocks",
                column: "draw_settings_id");

            migrationBuilder.CreateIndex(
                name: "IX_blocks_draw_table_id",
                table: "blocks",
                column: "draw_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_draw_tables_MainDrawSettingsId",
                table: "draw_tables",
                column: "MainDrawSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_draw_tables_QualifyingDrawSettingsId",
                table: "draw_tables",
                column: "QualifyingDrawSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_entry_details_draw_table_id",
                table: "entry_details",
                column: "draw_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_entry_players_entry_detail_id",
                table: "entry_players",
                column: "entry_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_results_GameId",
                table: "game_results",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_block_id",
                table: "games",
                column: "block_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_draw_settings_id",
                table: "games",
                column: "draw_settings_id");

            migrationBuilder.CreateIndex(
                name: "IX_opponents_game_id",
                table: "opponents",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_players_player_id",
                table: "request_players",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_players_season_id",
                table: "request_players",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_players_team_id",
                table: "request_players",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_teams_season_id",
                table: "request_teams",
                column: "season_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_teams_team_id",
                table: "request_teams",
                column: "team_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "authorization_links");

            migrationBuilder.DropTable(
                name: "entry_players");

            migrationBuilder.DropTable(
                name: "game_results");

            migrationBuilder.DropTable(
                name: "holding_dates");

            migrationBuilder.DropTable(
                name: "operators");

            migrationBuilder.DropTable(
                name: "opponents");

            migrationBuilder.DropTable(
                name: "request_players");

            migrationBuilder.DropTable(
                name: "request_teams");

            migrationBuilder.DropTable(
                name: "reservation_numbers");

            migrationBuilder.DropTable(
                name: "tennis_events");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "entry_details");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "seasons");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "tournaments");

            migrationBuilder.DropTable(
                name: "blocks");

            migrationBuilder.DropTable(
                name: "draw_tables");

            migrationBuilder.DropTable(
                name: "draw_settings");
        }
    }
}
