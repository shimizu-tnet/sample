export const Relationship = {
  None: 0,                            // 未定義
  Deceased: 1,                        // 被相続人
  Spouse: 2,                          // 配偶者
  FormerSpouse: 3,                    // 前夫前妻
  Child: 4,                           // 子
  FormerSpouseChild: 5,               // 子（前夫前妻）
  Adoption: 6,                        // 養子
  SpouseOfChild: 7,                   // 養子（子の配偶者）
  GrandchildAdoption: 8,              // 養子（孫養子）
  Grandchild: 9,                      // 孫
  Parent: 10,                         // 親
  Siblings: 11,                       // 兄弟姉妹
  HalfSiblings: 12,                   // 半血兄弟姉妹
  NephewOrNiece: 13,                  // 甥姪
  HalfNephewOrNiece: 14,              // 半血甥姪
  Mistress: 15,                       // 愛人
  MistressChild: 16,                  // 子（愛人）
  ParentFormerSpouse: 17,             // 親の前夫前妻
  SiblingsSpouse: 18,                 // 全血/半血兄弟姉妹の配偶者
}

const HeirPattern = {
  FirstPattern: 1,
  SecondPattern: 2,
  ThirdPattern: 3
};

export const getRelationshipName = relationship => {
  switch (Number(relationship)) {
    case Relationship.None:
      return "";
    case Relationship.Deceased:
      return "被相続人";
    case Relationship.Spouse:
      return "配偶者";
    case Relationship.FormerSpouse:
      return "前夫前妻";
    case Relationship.Child:
      return "子";
    case Relationship.FormerSpouseChild:
      return "子（前夫前妻）";
    case Relationship.Adoption:
      return "養子";
    case Relationship.SpouseOfChild:
      return "養子（子の配偶者）";
    case Relationship.GrandchildAdoption:
      return "養子（孫養子）";
    case Relationship.Grandchild:
      return "孫";
    case Relationship.Parent:
      return "親";
    case Relationship.Siblings:
      return "兄弟姉妹";
    case Relationship.HalfSiblings:
      return "半血兄弟姉妹";
    case Relationship.NephewOrNiece:
      return "甥姪";
    case Relationship.HalfNephewOrNiece:
      return "半血甥姪";
    case Relationship.Mistress:
      return "愛人";
    case Relationship.MistressChild:
      return "子（愛人）";
    case Relationship.ParentFormerSpouse:
      return "親の前夫前妻";
    case Relationship.SiblingsSpouse:
      return "全血/半血兄弟姉妹の配偶者";
    default:
      // 想定外の関係
      // 通るケース無し
      return null;
  }
};

export const getRelationshipsByHeirPattern = heirPattern => {
  switch (Number(heirPattern)) {
    case HeirPattern.FirstPattern:
      return [
        Relationship.None,                        // 未定義
        Relationship.Spouse,                      // 配偶者
        Relationship.FormerSpouse,                // 前夫前妻
        Relationship.Mistress,                    // 愛人
        Relationship.Child,                       // 子
        Relationship.FormerSpouseChild,           // 子（前夫前妻）
        Relationship.MistressChild,               // 子（愛人）
        Relationship.Adoption,                    // 養子
        Relationship.SpouseOfChild,               // 養子（子の配偶者）
        Relationship.GrandchildAdoption,          // 養子（孫養子）
        Relationship.Grandchild,                  // 孫
      ];
    case HeirPattern.SecondPattern:
      return [
        Relationship.None,                        // 未定義
        Relationship.Spouse,                      // 配偶者
        Relationship.Parent,                      // 親
      ];
    case HeirPattern.ThirdPattern:
      return [
        Relationship.None,                        // 未定義
        Relationship.Spouse,                      // 配偶者
        Relationship.Parent,                      // 親
        Relationship.Siblings,                    // 兄弟姉妹
        Relationship.HalfSiblings,                // 半血兄弟姉妹
        Relationship.NephewOrNiece,               // 甥姪
        Relationship.HalfNephewOrNiece,           // 半血甥姪
        Relationship.ParentFormerSpouse,          // 親の前夫前妻
        Relationship.SiblingsSpouse,              // 全血/半血兄弟姉妹の配偶者
      ];
    default:
      // 想定外の相続順位
      // 通るケース無し
      return null;
  }
}

export const getTargetRelativesRelationship = relationship => {
  switch (Number(relationship)) {
    // 未定義
    case Relationship.None:
      return [];
    // 被相続人
    case Relationship.Deceased:
      return [];
    // 配偶者
    case Relationship.Spouse:
      return [];
    // 前夫前妻
    case Relationship.FormerSpouse:
      return [];
    // 子
    case Relationship.Child:
      return [
        Relationship.Deceased,
        Relationship.Spouse,
      ];
    // 子（前夫前妻）
    case Relationship.FormerSpouseChild:
      return [
        Relationship.FormerSpouse,
      ];
    // 養子
    case Relationship.Adoption:
      return [
        Relationship.Deceased,
        Relationship.Spouse,
      ];
    // 養子（子の配偶者）
    case Relationship.SpouseOfChild:
      return [
        Relationship.Child
      ];
    // 養子（孫養子）
    case Relationship.GrandchildAdoption:
      return [
        Relationship.Child,
        Relationship.Adoption,
      ];
    // 孫
    case Relationship.Grandchild:
      return [
        Relationship.Child,
        Relationship.Adoption,
        Relationship.FormerSpouseChild,
        Relationship.MistressChild
      ];
    // 親
    case Relationship.Parent:
      return [];
    // 兄弟姉妹
    case Relationship.Siblings:
      return [];
    // 半血兄弟姉妹
    case Relationship.HalfSiblings:
      return [
        Relationship.ParentFormerSpouse
      ];
    // 甥姪
    case Relationship.NephewOrNiece:
      return [
        Relationship.Siblings
      ];
    // 半血甥姪
    case Relationship.HalfNephewOrNiece:
      return [
        Relationship.HalfSiblings
      ];
    // 子（愛人）
    case Relationship.MistressChild:
      return [
        Relationship.Mistress
      ];
    // 親の前夫前妻
    case Relationship.ParentFormerSpouse:
      return [
        Relationship.Parent
      ];
    // 全血/半血兄弟姉妹の配偶者
    case Relationship.SiblingsSpouse:
      return [
        Relationship.Siblings,
        Relationship.HalfSiblings,
      ];
    default:
      return [];
  }
}

export const isTargetRelative = targetRelativesRelationship => {
  const targetRelativesRelationships = getTargetRelativesRelationship(targetRelativesRelationship);
  return relative => {
    return targetRelativesRelationships.indexOf(Number(relative.relationship)) !== -1;
  }
};
export default Relationship;