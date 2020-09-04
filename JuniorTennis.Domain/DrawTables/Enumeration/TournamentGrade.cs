using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 大会グレードを定義します。
    /// </summary>
    public class TournamentGrade : Enumeration
    {
        /// <summary>
        /// グレード A。
        /// </summary>
        public static readonly TournamentGrade A = new TournamentGrade(1, "A");

        /// <summary>
        /// グレード B。
        /// </summary>
        public static readonly TournamentGrade B = new TournamentGrade(2, "B");

        /// <summary>
        /// グレード C。
        /// </summary>
        public static readonly TournamentGrade C = new TournamentGrade(3, "C");

        /// <summary>
        /// グレード D。
        /// </summary>
        public static readonly TournamentGrade D = new TournamentGrade(4, "D");

        /// <summary>
        /// グレード E。
        /// </summary>
        public static readonly TournamentGrade E = new TournamentGrade(5, "E");

        /// <summary>
        /// グレード F。
        /// </summary>
        public static readonly TournamentGrade F = new TournamentGrade(6, "F");

        /// <summary>
        /// グレード G。
        /// </summary>
        public static readonly TournamentGrade G = new TournamentGrade(7, "G");

        /// <summary>
        /// グレード H。
        /// </summary>
        public static readonly TournamentGrade H = new TournamentGrade(8, "H");

        /// <summary>
        /// グレード I。
        /// </summary>
        public static readonly TournamentGrade I = new TournamentGrade(9, "I");

        /// <summary>
        /// グレード J。
        /// </summary>
        public static readonly TournamentGrade J = new TournamentGrade(10, "J");

        /// <summary>
        /// グレード K。
        /// </summary>
        public static readonly TournamentGrade K = new TournamentGrade(11, "K");

        /// <summary>
        /// グレード L。
        /// </summary>
        public static readonly TournamentGrade L = new TournamentGrade(12, "L");

        /// <summary>
        /// グレード M。
        /// </summary>
        public static readonly TournamentGrade M = new TournamentGrade(13, "M");

        /// <summary>
        /// グレード N。
        /// </summary>
        public static readonly TournamentGrade N = new TournamentGrade(14, "N");

        /// <summary>
        /// グレード O。
        /// </summary>
        public static readonly TournamentGrade O = new TournamentGrade(15, "O");

        /// <summary>
        /// グレード P。
        /// </summary>
        public static readonly TournamentGrade P = new TournamentGrade(16, "P");

        /// <summary>
        /// グレード Q。
        /// </summary>
        public static readonly TournamentGrade Q = new TournamentGrade(17, "Q");

        /// <summary>
        /// グレード R。
        /// </summary>
        public static readonly TournamentGrade R = new TournamentGrade(18, "R");

        /// <summary>
        /// グレード S。
        /// </summary>
        public static readonly TournamentGrade S = new TournamentGrade(19, "S");

        /// <summary>
        /// グレード T。
        /// </summary>
        public static readonly TournamentGrade T = new TournamentGrade(20, "T");

        /// <summary>
        /// グレード U。
        /// </summary>
        public static readonly TournamentGrade U = new TournamentGrade(21, "U");

        /// <summary>
        /// グレード V。
        /// </summary>
        public static readonly TournamentGrade V = new TournamentGrade(22, "V");

        /// <summary>
        /// グレード W。
        /// </summary>
        public static readonly TournamentGrade W = new TournamentGrade(23, "W");

        /// <summary>
        /// グレード X。
        /// </summary>
        public static readonly TournamentGrade X = new TournamentGrade(24, "X");

        /// <summary>
        /// グレード Y。
        /// </summary>
        public static readonly TournamentGrade Y = new TournamentGrade(25, "Y");

        /// <summary>
        /// グレード Z。
        /// </summary>
        public static readonly TournamentGrade Z = new TournamentGrade(26, "Z");

        /// <summary>
        /// グレード AA。
        /// </summary>
        public static readonly TournamentGrade AA = new TournamentGrade(27, "AA");

        /// <summary>
        /// グレード AB。
        /// </summary>
        public static readonly TournamentGrade AB = new TournamentGrade(28, "AB");

        /// <summary>
        /// グレード AC。
        /// </summary>
        public static readonly TournamentGrade AC = new TournamentGrade(29, "AC");

        /// <summary>
        /// グレード AD。
        /// </summary>
        public static readonly TournamentGrade AD = new TournamentGrade(30, "AD");

        /// <summary>
        /// グレード AE。
        /// </summary>
        public static readonly TournamentGrade AE = new TournamentGrade(31, "AE");

        /// <summary>
        /// グレード AF。
        /// </summary>
        public static readonly TournamentGrade AF = new TournamentGrade(32, "AF");

        /// <summary>
        /// グレード AG。
        /// </summary>
        public static readonly TournamentGrade AG = new TournamentGrade(33, "AG");

        /// <summary>
        /// グレード AH。
        /// </summary>
        public static readonly TournamentGrade AH = new TournamentGrade(34, "AH");

        /// <summary>
        /// グレード AI。
        /// </summary>
        public static readonly TournamentGrade AI = new TournamentGrade(35, "AI");

        /// <summary>
        /// グレード AJ。
        /// </summary>
        public static readonly TournamentGrade AJ = new TournamentGrade(36, "AJ");

        /// <summary>
        /// グレード AK。
        /// </summary>
        public static readonly TournamentGrade AK = new TournamentGrade(37, "AK");

        /// <summary>
        /// グレード AL。
        /// </summary>
        public static readonly TournamentGrade AL = new TournamentGrade(38, "AL");

        /// <summary>
        /// グレード AM。
        /// </summary>
        public static readonly TournamentGrade AM = new TournamentGrade(39, "AM");

        /// <summary>
        /// グレード AN。
        /// </summary>
        public static readonly TournamentGrade AN = new TournamentGrade(40, "AN");

        /// <summary>
        /// グレード AO。
        /// </summary>
        public static readonly TournamentGrade AO = new TournamentGrade(41, "AO");

        /// <summary>
        /// グレード AP。
        /// </summary>
        public static readonly TournamentGrade AP = new TournamentGrade(42, "AP");

        /// <summary>
        /// グレード AQ。
        /// </summary>
        public static readonly TournamentGrade AQ = new TournamentGrade(43, "AQ");

        /// <summary>
        /// グレード AR。
        /// </summary>
        public static readonly TournamentGrade AR = new TournamentGrade(44, "AR");

        /// <summary>
        /// グレード AS。
        /// </summary>
        public static readonly TournamentGrade AS = new TournamentGrade(45, "AS");

        /// <summary>
        /// グレード AT。
        /// </summary>
        public static readonly TournamentGrade AT = new TournamentGrade(46, "AT");

        /// <summary>
        /// グレード AU。
        /// </summary>
        public static readonly TournamentGrade AU = new TournamentGrade(47, "AU");

        /// <summary>
        /// グレード AV。
        /// </summary>
        public static readonly TournamentGrade AV = new TournamentGrade(48, "AV");

        /// <summary>
        /// グレード AW。
        /// </summary>
        public static readonly TournamentGrade AW = new TournamentGrade(49, "AW");

        /// <summary>
        /// グレード AX。
        /// </summary>
        public static readonly TournamentGrade AX = new TournamentGrade(50, "AX");

        /// <summary>
        /// グレード AY。
        /// </summary>
        public static readonly TournamentGrade AY = new TournamentGrade(51, "AY");

        /// <summary>
        /// 大会グレードの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public TournamentGrade(int id, string name) : base(id, name) { }
    }
}
