namespace Canvas {

    /**
     * The css border style
    */
    export class Pen implements ICSSStyle {

        color: Color;
        width: number;

        /**
         * Create a new css border style for svg rectangle, line, etc.
         * 
         * @param color The border color
         * @param width The border width
        */
        constructor(color: Color, width: number = 1) {
            this.color = color;
            this.width = width;
        }

        Styling(node: SVGElement): SVGElement {
            node.style.stroke = this.color.ToHtmlColor();
            node.style.strokeWidth = this.width.toString();

            return node;
        }

        CSSStyle(): string {
            return `stroke-width:${this.width};stroke:${this.color.ToHtmlColor()};`;
        }

        //region "pens"
        
        /**
         * Black (#000000) 
        */
        static Black(width: number = 1) {
            return new Pen(Color.Black(), width);
        }

        /**
         * Night (#0C090A) 
        */
        static Night(width: number = 1) {
            return new Pen(Color.Night(), width);
        }

        /**
         * Gunmetal (#2C3539) 
        */
        static Gunmetal(width: number = 1) {
            return new Pen(Color.Gunmetal(), width);
        }

        /**
         * Midnight (#2B1B17) 
        */
        static Midnight(width: number = 1) {
            return new Pen(Color.Midnight(), width);
        }

        /**
         * Charcoal (#34282C) 
        */
        static Charcoal(width: number = 1) {
            return new Pen(Color.Charcoal(), width);
        }

        /**
         * Dark Slate Grey (#25383C) 
        */
        static DarkSlateGrey(width: number = 1) {
            return new Pen(Color.DarkSlateGrey(), width);
        }

        /**
         * Oil (#3B3131) 
        */
        static Oil(width: number = 1) {
            return new Pen(Color.Oil(), width);
        }

        /**
         * Black Cat (#413839) 
        */
        static BlackCat(width: number = 1) {
            return new Pen(Color.BlackCat(), width);
        }

        /**
         * Iridium (#3D3C3A) 
        */
        static Iridium(width: number = 1) {
            return new Pen(Color.Iridium(), width);
        }

        /**
         * Black Eel (#463E3F) 
        */
        static BlackEel(width: number = 1) {
            return new Pen(Color.BlackEel(), width);
        }

        /**
         * Black Cow (#4C4646) 
        */
        static BlackCow(width: number = 1) {
            return new Pen(Color.BlackCow(), width);
        }

        /**
         * Gray Wolf (#504A4B) 
        */
        static GrayWolf(width: number = 1) {
            return new Pen(Color.GrayWolf(), width);
        }

        /**
         * Vampire Gray (#565051) 
        */
        static VampireGray(width: number = 1) {
            return new Pen(Color.VampireGray(), width);
        }

        /**
         * Gray Dolphin (#5C5858) 
        */
        static GrayDolphin(width: number = 1) {
            return new Pen(Color.GrayDolphin(), width);
        }

        /**
         * Carbon Gray (#625D5D) 
        */
        static CarbonGray(width: number = 1) {
            return new Pen(Color.CarbonGray(), width);
        }

        /**
         * Ash Gray (#666362) 
        */
        static AshGray(width: number = 1) {
            return new Pen(Color.AshGray(), width);
        }

        /**
         * Cloudy Gray (#6D6968) 
        */
        static CloudyGray(width: number = 1) {
            return new Pen(Color.CloudyGray(), width);
        }

        /**
         * Smokey Gray (#726E6D) 
        */
        static SmokeyGray(width: number = 1) {
            return new Pen(Color.SmokeyGray(), width);
        }

        /**
         * Gray (#736F6E) 
        */
        static Gray(width: number = 1) {
            return new Pen(Color.Gray(), width);
        }

        /**
         * Granite (#837E7C) 
        */
        static Granite(width: number = 1) {
            return new Pen(Color.Granite(), width);
        }

        /**
         * Battleship Gray (#848482) 
        */
        static BattleshipGray(width: number = 1) {
            return new Pen(Color.BattleshipGray(), width);
        }

        /**
         * Gray Cloud (#B6B6B4) 
        */
        static GrayCloud(width: number = 1) {
            return new Pen(Color.GrayCloud(), width);
        }

        /**
         * Gray Goose (#D1D0CE) 
        */
        static GrayGoose(width: number = 1) {
            return new Pen(Color.GrayGoose(), width);
        }

        /**
         * Platinum (#E5E4E2) 
        */
        static Platinum(width: number = 1) {
            return new Pen(Color.Platinum(), width);
        }

        /**
         * Metallic Silver (#BCC6CC) 
        */
        static MetallicSilver(width: number = 1) {
            return new Pen(Color.MetallicSilver(), width);
        }

        /**
         * Blue Gray (#98AFC7) 
        */
        static BlueGray(width: number = 1) {
            return new Pen(Color.BlueGray(), width);
        }

        /**
         * Light Slate Gray (#6D7B8D) 
        */
        static LightSlateGray(width: number = 1) {
            return new Pen(Color.LightSlateGray(), width);
        }

        /**
         * Slate Gray (#657383) 
        */
        static SlateGray(width: number = 1) {
            return new Pen(Color.SlateGray(), width);
        }

        /**
         * Jet Gray (#616D7E) 
        */
        static JetGray(width: number = 1) {
            return new Pen(Color.JetGray(), width);
        }

        /**
         * Mist Blue (#646D7E) 
        */
        static MistBlue(width: number = 1) {
            return new Pen(Color.MistBlue(), width);
        }

        /**
         * Marble Blue (#566D7E) 
        */
        static MarbleBlue(width: number = 1) {
            return new Pen(Color.MarbleBlue(), width);
        }

        /**
         * Slate Blue (#737CA1) 
        */
        static SlateBlue(width: number = 1) {
            return new Pen(Color.SlateBlue(), width);
        }

        /**
         * Steel Blue (#4863A0) 
        */
        static SteelBlue(width: number = 1) {
            return new Pen(Color.SteelBlue(), width);
        }

        /**
         * Blue Jay (#2B547E) 
        */
        static BlueJay(width: number = 1) {
            return new Pen(Color.BlueJay(), width);
        }

        /**
         * Dark Slate Blue (#2B3856) 
        */
        static DarkSlateBlue(width: number = 1) {
            return new Pen(Color.DarkSlateBlue(), width);
        }

        /**
         * Midnight Blue (#151B54) 
        */
        static MidnightBlue(width: number = 1) {
            return new Pen(Color.MidnightBlue(), width);
        }

        /**
         * Navy Blue (#000080) 
        */
        static NavyBlue(width: number = 1) {
            return new Pen(Color.NavyBlue(), width);
        }

        /**
         * Blue Whale (#342D7E) 
        */
        static BlueWhale(width: number = 1) {
            return new Pen(Color.BlueWhale(), width);
        }

        /**
         * Lapis Blue (#15317E) 
        */
        static LapisBlue(width: number = 1) {
            return new Pen(Color.LapisBlue(), width);
        }

        /**
         * Denim Dark Blue (#151B8D) 
        */
        static DenimDarkBlue(width: number = 1) {
            return new Pen(Color.DenimDarkBlue(), width);
        }

        /**
         * Earth Blue (#0000A0) 
        */
        static EarthBlue(width: number = 1) {
            return new Pen(Color.EarthBlue(), width);
        }

        /**
         * Cobalt Blue (#0020C2) 
        */
        static CobaltBlue(width: number = 1) {
            return new Pen(Color.CobaltBlue(), width);
        }

        /**
         * Blueberry Blue (#0041C2) 
        */
        static BlueberryBlue(width: number = 1) {
            return new Pen(Color.BlueberryBlue(), width);
        }

        /**
         * Sapphire Blue (#2554C7) 
        */
        static SapphireBlue(width: number = 1) {
            return new Pen(Color.SapphireBlue(), width);
        }

        /**
         * Blue Eyes (#1569C7) 
        */
        static BlueEyes(width: number = 1) {
            return new Pen(Color.BlueEyes(), width);
        }

        /**
         * Royal Blue (#2B60DE) 
        */
        static RoyalBlue(width: number = 1) {
            return new Pen(Color.RoyalBlue(), width);
        }

        /**
         * Blue Orchid (#1F45FC) 
        */
        static BlueOrchid(width: number = 1) {
            return new Pen(Color.BlueOrchid(), width);
        }

        /**
         * Blue Lotus (#6960EC) 
        */
        static BlueLotus(width: number = 1) {
            return new Pen(Color.BlueLotus(), width);
        }

        /**
         * Light Slate Blue (#736AFF) 
        */
        static LightSlateBlue(width: number = 1) {
            return new Pen(Color.LightSlateBlue(), width);
        }

        /**
         * Windows Blue (#357EC7) 
        */
        static WindowsBlue(width: number = 1) {
            return new Pen(Color.WindowsBlue(), width);
        }

        /**
         * Glacial Blue Ice (#368BC1) 
        */
        static GlacialBlueIce(width: number = 1) {
            return new Pen(Color.GlacialBlueIce(), width);
        }

        /**
         * Silk Blue (#488AC7) 
        */
        static SilkBlue(width: number = 1) {
            return new Pen(Color.SilkBlue(), width);
        }

        /**
         * Blue Ivy (#3090C7) 
        */
        static BlueIvy(width: number = 1) {
            return new Pen(Color.BlueIvy(), width);
        }

        /**
         * Blue Koi (#659EC7) 
        */
        static BlueKoi(width: number = 1) {
            return new Pen(Color.BlueKoi(), width);
        }

        /**
         * Columbia Blue (#87AFC7) 
        */
        static ColumbiaBlue(width: number = 1) {
            return new Pen(Color.ColumbiaBlue(), width);
        }

        /**
         * Baby Blue (#95B9C7) 
        */
        static BabyBlue(width: number = 1) {
            return new Pen(Color.BabyBlue(), width);
        }

        /**
         * Light Steel Blue (#728FCE) 
        */
        static LightSteelBlue(width: number = 1) {
            return new Pen(Color.LightSteelBlue(), width);
        }

        /**
         * Ocean Blue (#2B65EC) 
        */
        static OceanBlue(width: number = 1) {
            return new Pen(Color.OceanBlue(), width);
        }

        /**
         * Blue Ribbon (#306EFF) 
        */
        static BlueRibbon(width: number = 1) {
            return new Pen(Color.BlueRibbon(), width);
        }

        /**
         * Blue Dress (#157DEC) 
        */
        static BlueDress(width: number = 1) {
            return new Pen(Color.BlueDress(), width);
        }

        /**
         * Dodger Blue (#1589FF) 
        */
        static DodgerBlue(width: number = 1) {
            return new Pen(Color.DodgerBlue(), width);
        }

        /**
         * Cornflower Blue (#6495ED) 
        */
        static CornflowerBlue(width: number = 1) {
            return new Pen(Color.CornflowerBlue(), width);
        }

        /**
         * Sky Blue (#6698FF) 
        */
        static SkyBlue(width: number = 1) {
            return new Pen(Color.SkyBlue(), width);
        }

        /**
         * Butterfly Blue (#38ACEC) 
        */
        static ButterflyBlue(width: number = 1) {
            return new Pen(Color.ButterflyBlue(), width);
        }

        /**
         * Iceberg (#56A5EC) 
        */
        static Iceberg(width: number = 1) {
            return new Pen(Color.Iceberg(), width);
        }

        /**
         * Crystal Blue (#5CB3FF) 
        */
        static CrystalBlue(width: number = 1) {
            return new Pen(Color.CrystalBlue(), width);
        }

        /**
         * Deep Sky Blue (#3BB9FF) 
        */
        static DeepSkyBlue(width: number = 1) {
            return new Pen(Color.DeepSkyBlue(), width);
        }

        /**
         * Denim Blue (#79BAEC) 
        */
        static DenimBlue(width: number = 1) {
            return new Pen(Color.DenimBlue(), width);
        }

        /**
         * Light Sky Blue (#82CAFA) 
        */
        static LightSkyBlue(width: number = 1) {
            return new Pen(Color.LightSkyBlue(), width);
        }

        /**
         * Day Sky Blue (#82CAFF) 
        */
        static DaySkyBlue(width: number = 1) {
            return new Pen(Color.DaySkyBlue(), width);
        }

        /**
         * Jeans Blue (#A0CFEC) 
        */
        static JeansBlue(width: number = 1) {
            return new Pen(Color.JeansBlue(), width);
        }

        /**
         * Blue Angel (#B7CEEC) 
        */
        static BlueAngel(width: number = 1) {
            return new Pen(Color.BlueAngel(), width);
        }

        /**
         * Pastel Blue (#B4CFEC) 
        */
        static PastelBlue(width: number = 1) {
            return new Pen(Color.PastelBlue(), width);
        }

        /**
         * Sea Blue (#C2DFFF) 
        */
        static SeaBlue(width: number = 1) {
            return new Pen(Color.SeaBlue(), width);
        }

        /**
         * Powder Blue (#C6DEFF) 
        */
        static PowderBlue(width: number = 1) {
            return new Pen(Color.PowderBlue(), width);
        }

        /**
         * Coral Blue (#AFDCEC) 
        */
        static CoralBlue(width: number = 1) {
            return new Pen(Color.CoralBlue(), width);
        }

        /**
         * Light Blue (#ADDFFF) 
        */
        static LightBlue(width: number = 1) {
            return new Pen(Color.LightBlue(), width);
        }

        /**
         * Robin Egg Blue (#BDEDFF) 
        */
        static RobinEggBlue(width: number = 1) {
            return new Pen(Color.RobinEggBlue(), width);
        }

        /**
         * Pale Blue Lily (#CFECEC) 
        */
        static PaleBlueLily(width: number = 1) {
            return new Pen(Color.PaleBlueLily(), width);
        }

        /**
         * Light Cyan (#E0FFFF) 
        */
        static LightCyan(width: number = 1) {
            return new Pen(Color.LightCyan(), width);
        }

        /**
         * Water (#EBF4FA) 
        */
        static Water(width: number = 1) {
            return new Pen(Color.Water(), width);
        }

        /**
         * AliceBlue (#F0F8FF) 
        */
        static AliceBlue(width: number = 1) {
            return new Pen(Color.AliceBlue(), width);
        }

        /**
         * Azure (#F0FFFF) 
        */
        static Azure(width: number = 1) {
            return new Pen(Color.Azure(), width);
        }

        /**
         * Light Slate (#CCFFFF) 
        */
        static LightSlate(width: number = 1) {
            return new Pen(Color.LightSlate(), width);
        }

        /**
         * Light Aquamarine (#93FFE8) 
        */
        static LightAquamarine(width: number = 1) {
            return new Pen(Color.LightAquamarine(), width);
        }

        /**
         * Electric Blue (#9AFEFF) 
        */
        static ElectricBlue(width: number = 1) {
            return new Pen(Color.ElectricBlue(), width);
        }

        /**
         * Aquamarine (#7FFFD4) 
        */
        static Aquamarine(width: number = 1) {
            return new Pen(Color.Aquamarine(), width);
        }

        /**
         * Cyan or Aqua (#00FFFF) 
        */
        static CyanorAqua(width: number = 1) {
            return new Pen(Color.CyanorAqua(), width);
        }

        /**
         * Tron Blue (#7DFDFE) 
        */
        static TronBlue(width: number = 1) {
            return new Pen(Color.TronBlue(), width);
        }

        /**
         * Blue Zircon (#57FEFF) 
        */
        static BlueZircon(width: number = 1) {
            return new Pen(Color.BlueZircon(), width);
        }

        /**
         * Blue Lagoon (#8EEBEC) 
        */
        static BlueLagoon(width: number = 1) {
            return new Pen(Color.BlueLagoon(), width);
        }

        /**
         * Celeste (#50EBEC) 
        */
        static Celeste(width: number = 1) {
            return new Pen(Color.Celeste(), width);
        }

        /**
         * Blue Diamond (#4EE2EC) 
        */
        static BlueDiamond(width: number = 1) {
            return new Pen(Color.BlueDiamond(), width);
        }

        /**
         * Tiffany Blue (#81D8D0) 
        */
        static TiffanyBlue(width: number = 1) {
            return new Pen(Color.TiffanyBlue(), width);
        }

        /**
         * Cyan Opaque (#92C7C7) 
        */
        static CyanOpaque(width: number = 1) {
            return new Pen(Color.CyanOpaque(), width);
        }

        /**
         * Blue Hosta (#77BFC7) 
        */
        static BlueHosta(width: number = 1) {
            return new Pen(Color.BlueHosta(), width);
        }

        /**
         * Northern Lights Blue (#78C7C7) 
        */
        static NorthernLightsBlue(width: number = 1) {
            return new Pen(Color.NorthernLightsBlue(), width);
        }

        /**
         * Medium Turquoise (#48CCCD) 
        */
        static MediumTurquoise(width: number = 1) {
            return new Pen(Color.MediumTurquoise(), width);
        }

        /**
         * Turquoise (#43C6DB) 
        */
        static Turquoise(width: number = 1) {
            return new Pen(Color.Turquoise(), width);
        }

        /**
         * Jellyfish (#46C7C7) 
        */
        static Jellyfish(width: number = 1) {
            return new Pen(Color.Jellyfish(), width);
        }

        /**
         * Blue green (#7BCCB5) 
        */
        static Bluegreen(width: number = 1) {
            return new Pen(Color.Bluegreen(), width);
        }

        /**
         * Macaw Blue Green (#43BFC7) 
        */
        static MacawBlueGreen(width: number = 1) {
            return new Pen(Color.MacawBlueGreen(), width);
        }

        /**
         * Light Sea Green (#3EA99F) 
        */
        static LightSeaGreen(width: number = 1) {
            return new Pen(Color.LightSeaGreen(), width);
        }

        /**
         * Dark Turquoise (#3B9C9C) 
        */
        static DarkTurquoise(width: number = 1) {
            return new Pen(Color.DarkTurquoise(), width);
        }

        /**
         * Sea Turtle Green (#438D80) 
        */
        static SeaTurtleGreen(width: number = 1) {
            return new Pen(Color.SeaTurtleGreen(), width);
        }

        /**
         * Medium Aquamarine (#348781) 
        */
        static MediumAquamarine(width: number = 1) {
            return new Pen(Color.MediumAquamarine(), width);
        }

        /**
         * Greenish Blue (#307D7E) 
        */
        static GreenishBlue(width: number = 1) {
            return new Pen(Color.GreenishBlue(), width);
        }

        /**
         * Grayish Turquoise (#5E7D7E) 
        */
        static GrayishTurquoise(width: number = 1) {
            return new Pen(Color.GrayishTurquoise(), width);
        }

        /**
         * Beetle Green (#4C787E) 
        */
        static BeetleGreen(width: number = 1) {
            return new Pen(Color.BeetleGreen(), width);
        }

        /**
         * Teal (#008080) 
        */
        static Teal(width: number = 1) {
            return new Pen(Color.Teal(), width);
        }

        /**
         * Sea Green (#4E8975) 
        */
        static SeaGreen(width: number = 1) {
            return new Pen(Color.SeaGreen(), width);
        }

        /**
         * Camouflage Green (#78866B) 
        */
        static CamouflageGreen(width: number = 1) {
            return new Pen(Color.CamouflageGreen(), width);
        }

        /**
         * Sage Green (#848b79) 
        */
        static SageGreen(width: number = 1) {
            return new Pen(Color.SageGreen(), width);
        }

        /**
         * Hazel Green (#617C58) 
        */
        static HazelGreen(width: number = 1) {
            return new Pen(Color.HazelGreen(), width);
        }

        /**
         * Venom Green (#728C00) 
        */
        static VenomGreen(width: number = 1) {
            return new Pen(Color.VenomGreen(), width);
        }

        /**
         * Fern Green (#667C26) 
        */
        static FernGreen(width: number = 1) {
            return new Pen(Color.FernGreen(), width);
        }

        /**
         * Dark Forest Green (#254117) 
        */
        static DarkForestGreen(width: number = 1) {
            return new Pen(Color.DarkForestGreen(), width);
        }

        /**
         * Medium Sea Green (#306754) 
        */
        static MediumSeaGreen(width: number = 1) {
            return new Pen(Color.MediumSeaGreen(), width);
        }

        /**
         * Medium Forest Green (#347235) 
        */
        static MediumForestGreen(width: number = 1) {
            return new Pen(Color.MediumForestGreen(), width);
        }

        /**
         * Seaweed Green (#437C17) 
        */
        static SeaweedGreen(width: number = 1) {
            return new Pen(Color.SeaweedGreen(), width);
        }

        /**
         * Pine Green (#387C44) 
        */
        static PineGreen(width: number = 1) {
            return new Pen(Color.PineGreen(), width);
        }

        /**
         * Jungle Green (#347C2C) 
        */
        static JungleGreen(width: number = 1) {
            return new Pen(Color.JungleGreen(), width);
        }

        /**
         * Shamrock Green (#347C17) 
        */
        static ShamrockGreen(width: number = 1) {
            return new Pen(Color.ShamrockGreen(), width);
        }

        /**
         * Medium Spring Green (#348017) 
        */
        static MediumSpringGreen(width: number = 1) {
            return new Pen(Color.MediumSpringGreen(), width);
        }

        /**
         * Forest Green (#4E9258) 
        */
        static ForestGreen(width: number = 1) {
            return new Pen(Color.ForestGreen(), width);
        }

        /**
         * Green Onion (#6AA121) 
        */
        static GreenOnion(width: number = 1) {
            return new Pen(Color.GreenOnion(), width);
        }

        /**
         * Spring Green (#4AA02C) 
        */
        static SpringGreen(width: number = 1) {
            return new Pen(Color.SpringGreen(), width);
        }

        /**
         * Lime Green (#41A317) 
        */
        static LimeGreen(width: number = 1) {
            return new Pen(Color.LimeGreen(), width);
        }

        /**
         * Clover Green (#3EA055) 
        */
        static CloverGreen(width: number = 1) {
            return new Pen(Color.CloverGreen(), width);
        }

        /**
         * Green Snake (#6CBB3C) 
        */
        static GreenSnake(width: number = 1) {
            return new Pen(Color.GreenSnake(), width);
        }

        /**
         * Alien Green (#6CC417) 
        */
        static AlienGreen(width: number = 1) {
            return new Pen(Color.AlienGreen(), width);
        }

        /**
         * Green Apple (#4CC417) 
        */
        static GreenApple(width: number = 1) {
            return new Pen(Color.GreenApple(), width);
        }

        /**
         * Yellow Green (#52D017) 
        */
        static YellowGreen(width: number = 1) {
            return new Pen(Color.YellowGreen(), width);
        }

        /**
         * Kelly Green (#4CC552) 
        */
        static KellyGreen(width: number = 1) {
            return new Pen(Color.KellyGreen(), width);
        }

        /**
         * Zombie Green (#54C571) 
        */
        static ZombieGreen(width: number = 1) {
            return new Pen(Color.ZombieGreen(), width);
        }

        /**
         * Frog Green (#99C68E) 
        */
        static FrogGreen(width: number = 1) {
            return new Pen(Color.FrogGreen(), width);
        }

        /**
         * Green Peas (#89C35C) 
        */
        static GreenPeas(width: number = 1) {
            return new Pen(Color.GreenPeas(), width);
        }

        /**
         * Dollar Bill Green (#85BB65) 
        */
        static DollarBillGreen(width: number = 1) {
            return new Pen(Color.DollarBillGreen(), width);
        }

        /**
         * Dark Sea Green (#8BB381) 
        */
        static DarkSeaGreen(width: number = 1) {
            return new Pen(Color.DarkSeaGreen(), width);
        }

        /**
         * Iguana Green (#9CB071) 
        */
        static IguanaGreen(width: number = 1) {
            return new Pen(Color.IguanaGreen(), width);
        }

        /**
         * Avocado Green (#B2C248) 
        */
        static AvocadoGreen(width: number = 1) {
            return new Pen(Color.AvocadoGreen(), width);
        }

        /**
         * Pistachio Green (#9DC209) 
        */
        static PistachioGreen(width: number = 1) {
            return new Pen(Color.PistachioGreen(), width);
        }

        /**
         * Salad Green (#A1C935) 
        */
        static SaladGreen(width: number = 1) {
            return new Pen(Color.SaladGreen(), width);
        }

        /**
         * Hummingbird Green (#7FE817) 
        */
        static HummingbirdGreen(width: number = 1) {
            return new Pen(Color.HummingbirdGreen(), width);
        }

        /**
         * Nebula Green (#59E817) 
        */
        static NebulaGreen(width: number = 1) {
            return new Pen(Color.NebulaGreen(), width);
        }

        /**
         * Stoplight Go Green (#57E964) 
        */
        static StoplightGoGreen(width: number = 1) {
            return new Pen(Color.StoplightGoGreen(), width);
        }

        /**
         * Algae Green (#64E986) 
        */
        static AlgaeGreen(width: number = 1) {
            return new Pen(Color.AlgaeGreen(), width);
        }

        /**
         * Jade Green (#5EFB6E) 
        */
        static JadeGreen(width: number = 1) {
            return new Pen(Color.JadeGreen(), width);
        }

        /**
         * Green (#00FF00) 
        */
        static Green(width: number = 1) {
            return new Pen(Color.Green(), width);
        }

        /**
         * Emerald Green (#5FFB17) 
        */
        static EmeraldGreen(width: number = 1) {
            return new Pen(Color.EmeraldGreen(), width);
        }

        /**
         * Lawn Green (#87F717) 
        */
        static LawnGreen(width: number = 1) {
            return new Pen(Color.LawnGreen(), width);
        }

        /**
         * Chartreuse (#8AFB17) 
        */
        static Chartreuse(width: number = 1) {
            return new Pen(Color.Chartreuse(), width);
        }

        /**
         * Dragon Green (#6AFB92) 
        */
        static DragonGreen(width: number = 1) {
            return new Pen(Color.DragonGreen(), width);
        }

        /**
         * Mint green (#98FF98) 
        */
        static Mintgreen(width: number = 1) {
            return new Pen(Color.Mintgreen(), width);
        }

        /**
         * Green Thumb (#B5EAAA) 
        */
        static GreenThumb(width: number = 1) {
            return new Pen(Color.GreenThumb(), width);
        }

        /**
         * Light Jade (#C3FDB8) 
        */
        static LightJade(width: number = 1) {
            return new Pen(Color.LightJade(), width);
        }

        /**
         * Tea Green (#CCFB5D) 
        */
        static TeaGreen(width: number = 1) {
            return new Pen(Color.TeaGreen(), width);
        }

        /**
         * Green Yellow (#B1FB17) 
        */
        static GreenYellow(width: number = 1) {
            return new Pen(Color.GreenYellow(), width);
        }

        /**
         * Slime Green (#BCE954) 
        */
        static SlimeGreen(width: number = 1) {
            return new Pen(Color.SlimeGreen(), width);
        }

        /**
         * Goldenrod (#EDDA74) 
        */
        static Goldenrod(width: number = 1) {
            return new Pen(Color.Goldenrod(), width);
        }

        /**
         * Harvest Gold (#EDE275) 
        */
        static HarvestGold(width: number = 1) {
            return new Pen(Color.HarvestGold(), width);
        }

        /**
         * Sun Yellow (#FFE87C) 
        */
        static SunYellow(width: number = 1) {
            return new Pen(Color.SunYellow(), width);
        }

        /**
         * Yellow (#FFFF00) 
        */
        static Yellow(width: number = 1) {
            return new Pen(Color.Yellow(), width);
        }

        /**
         * Corn Yellow (#FFF380) 
        */
        static CornYellow(width: number = 1) {
            return new Pen(Color.CornYellow(), width);
        }

        /**
         * Parchment (#FFFFC2) 
        */
        static Parchment(width: number = 1) {
            return new Pen(Color.Parchment(), width);
        }

        /**
         * Cream (#FFFFCC) 
        */
        static Cream(width: number = 1) {
            return new Pen(Color.Cream(), width);
        }

        /**
         * Lemon Chiffon (#FFF8C6) 
        */
        static LemonChiffon(width: number = 1) {
            return new Pen(Color.LemonChiffon(), width);
        }

        /**
         * Cornsilk (#FFF8DC) 
        */
        static Cornsilk(width: number = 1) {
            return new Pen(Color.Cornsilk(), width);
        }

        /**
         * Beige (#F5F5DC) 
        */
        static Beige(width: number = 1) {
            return new Pen(Color.Beige(), width);
        }

        /**
         * Blonde (#FBF6D9) 
        */
        static Blonde(width: number = 1) {
            return new Pen(Color.Blonde(), width);
        }

        /**
         * AntiqueWhite (#FAEBD7) 
        */
        static AntiqueWhite(width: number = 1) {
            return new Pen(Color.AntiqueWhite(), width);
        }

        /**
         * Champagne (#F7E7CE) 
        */
        static Champagne(width: number = 1) {
            return new Pen(Color.Champagne(), width);
        }

        /**
         * BlanchedAlmond (#FFEBCD) 
        */
        static BlanchedAlmond(width: number = 1) {
            return new Pen(Color.BlanchedAlmond(), width);
        }

        /**
         * Vanilla (#F3E5AB) 
        */
        static Vanilla(width: number = 1) {
            return new Pen(Color.Vanilla(), width);
        }

        /**
         * Tan Brown (#ECE5B6) 
        */
        static TanBrown(width: number = 1) {
            return new Pen(Color.TanBrown(), width);
        }

        /**
         * Peach (#FFE5B4) 
        */
        static Peach(width: number = 1) {
            return new Pen(Color.Peach(), width);
        }

        /**
         * Mustard (#FFDB58) 
        */
        static Mustard(width: number = 1) {
            return new Pen(Color.Mustard(), width);
        }

        /**
         * Rubber Ducky Yellow (#FFD801) 
        */
        static RubberDuckyYellow(width: number = 1) {
            return new Pen(Color.RubberDuckyYellow(), width);
        }

        /**
         * Bright Gold (#FDD017) 
        */
        static BrightGold(width: number = 1) {
            return new Pen(Color.BrightGold(), width);
        }

        /**
         * Golden brown (#EAC117) 
        */
        static Goldenbrown(width: number = 1) {
            return new Pen(Color.Goldenbrown(), width);
        }

        /**
         * Macaroni and Cheese (#F2BB66) 
        */
        static MacaroniandCheese(width: number = 1) {
            return new Pen(Color.MacaroniandCheese(), width);
        }

        /**
         * Saffron (#FBB917) 
        */
        static Saffron(width: number = 1) {
            return new Pen(Color.Saffron(), width);
        }

        /**
         * Beer (#FBB117) 
        */
        static Beer(width: number = 1) {
            return new Pen(Color.Beer(), width);
        }

        /**
         * Cantaloupe (#FFA62F) 
        */
        static Cantaloupe(width: number = 1) {
            return new Pen(Color.Cantaloupe(), width);
        }

        /**
         * Bee Yellow (#E9AB17) 
        */
        static BeeYellow(width: number = 1) {
            return new Pen(Color.BeeYellow(), width);
        }

        /**
         * Brown Sugar (#E2A76F) 
        */
        static BrownSugar(width: number = 1) {
            return new Pen(Color.BrownSugar(), width);
        }

        /**
         * BurlyWood (#DEB887) 
        */
        static BurlyWood(width: number = 1) {
            return new Pen(Color.BurlyWood(), width);
        }

        /**
         * Deep Peach (#FFCBA4) 
        */
        static DeepPeach(width: number = 1) {
            return new Pen(Color.DeepPeach(), width);
        }

        /**
         * Ginger Brown (#C9BE62) 
        */
        static GingerBrown(width: number = 1) {
            return new Pen(Color.GingerBrown(), width);
        }

        /**
         * School Bus Yellow (#E8A317) 
        */
        static SchoolBusYellow(width: number = 1) {
            return new Pen(Color.SchoolBusYellow(), width);
        }

        /**
         * Sandy Brown (#EE9A4D) 
        */
        static SandyBrown(width: number = 1) {
            return new Pen(Color.SandyBrown(), width);
        }

        /**
         * Fall Leaf Brown (#C8B560) 
        */
        static FallLeafBrown(width: number = 1) {
            return new Pen(Color.FallLeafBrown(), width);
        }

        /**
         * Orange Gold (#D4A017) 
        */
        static OrangeGold(width: number = 1) {
            return new Pen(Color.OrangeGold(), width);
        }

        /**
         * Sand (#C2B280) 
        */
        static Sand(width: number = 1) {
            return new Pen(Color.Sand(), width);
        }

        /**
         * Cookie Brown (#C7A317) 
        */
        static CookieBrown(width: number = 1) {
            return new Pen(Color.CookieBrown(), width);
        }

        /**
         * Caramel (#C68E17) 
        */
        static Caramel(width: number = 1) {
            return new Pen(Color.Caramel(), width);
        }

        /**
         * Brass (#B5A642) 
        */
        static Brass(width: number = 1) {
            return new Pen(Color.Brass(), width);
        }

        /**
         * Khaki (#ADA96E) 
        */
        static Khaki(width: number = 1) {
            return new Pen(Color.Khaki(), width);
        }

        /**
         * Camel brown (#C19A6B) 
        */
        static Camelbrown(width: number = 1) {
            return new Pen(Color.Camelbrown(), width);
        }

        /**
         * Bronze (#CD7F32) 
        */
        static Bronze(width: number = 1) {
            return new Pen(Color.Bronze(), width);
        }

        /**
         * Tiger Orange (#C88141) 
        */
        static TigerOrange(width: number = 1) {
            return new Pen(Color.TigerOrange(), width);
        }

        /**
         * Cinnamon (#C58917) 
        */
        static Cinnamon(width: number = 1) {
            return new Pen(Color.Cinnamon(), width);
        }

        /**
         * Bullet Shell (#AF9B60) 
        */
        static BulletShell(width: number = 1) {
            return new Pen(Color.BulletShell(), width);
        }

        /**
         * Dark Goldenrod (#AF7817) 
        */
        static DarkGoldenrod(width: number = 1) {
            return new Pen(Color.DarkGoldenrod(), width);
        }

        /**
         * Copper (#B87333) 
        */
        static Copper(width: number = 1) {
            return new Pen(Color.Copper(), width);
        }

        /**
         * Wood (#966F33) 
        */
        static Wood(width: number = 1) {
            return new Pen(Color.Wood(), width);
        }

        /**
         * Oak Brown (#806517) 
        */
        static OakBrown(width: number = 1) {
            return new Pen(Color.OakBrown(), width);
        }

        /**
         * Moccasin (#827839) 
        */
        static Moccasin(width: number = 1) {
            return new Pen(Color.Moccasin(), width);
        }

        /**
         * Army Brown (#827B60) 
        */
        static ArmyBrown(width: number = 1) {
            return new Pen(Color.ArmyBrown(), width);
        }

        /**
         * Sandstone (#786D5F) 
        */
        static Sandstone(width: number = 1) {
            return new Pen(Color.Sandstone(), width);
        }

        /**
         * Mocha (#493D26) 
        */
        static Mocha(width: number = 1) {
            return new Pen(Color.Mocha(), width);
        }

        /**
         * Taupe (#483C32) 
        */
        static Taupe(width: number = 1) {
            return new Pen(Color.Taupe(), width);
        }

        /**
         * Coffee (#6F4E37) 
        */
        static Coffee(width: number = 1) {
            return new Pen(Color.Coffee(), width);
        }

        /**
         * Brown Bear (#835C3B) 
        */
        static BrownBear(width: number = 1) {
            return new Pen(Color.BrownBear(), width);
        }

        /**
         * Red Dirt (#7F5217) 
        */
        static RedDirt(width: number = 1) {
            return new Pen(Color.RedDirt(), width);
        }

        /**
         * Sepia (#7F462C) 
        */
        static Sepia(width: number = 1) {
            return new Pen(Color.Sepia(), width);
        }

        /**
         * Orange Salmon (#C47451) 
        */
        static OrangeSalmon(width: number = 1) {
            return new Pen(Color.OrangeSalmon(), width);
        }

        /**
         * Rust (#C36241) 
        */
        static Rust(width: number = 1) {
            return new Pen(Color.Rust(), width);
        }

        /**
         * Red Fox (#C35817) 
        */
        static RedFox(width: number = 1) {
            return new Pen(Color.RedFox(), width);
        }

        /**
         * Chocolate (#C85A17) 
        */
        static Chocolate(width: number = 1) {
            return new Pen(Color.Chocolate(), width);
        }

        /**
         * Sedona (#CC6600) 
        */
        static Sedona(width: number = 1) {
            return new Pen(Color.Sedona(), width);
        }

        /**
         * Papaya Orange (#E56717) 
        */
        static PapayaOrange(width: number = 1) {
            return new Pen(Color.PapayaOrange(), width);
        }

        /**
         * Halloween Orange (#E66C2C) 
        */
        static HalloweenOrange(width: number = 1) {
            return new Pen(Color.HalloweenOrange(), width);
        }

        /**
         * Pumpkin Orange (#F87217) 
        */
        static PumpkinOrange(width: number = 1) {
            return new Pen(Color.PumpkinOrange(), width);
        }

        /**
         * Construction Cone Orange (#F87431) 
        */
        static ConstructionConeOrange(width: number = 1) {
            return new Pen(Color.ConstructionConeOrange(), width);
        }

        /**
         * Sunrise Orange (#E67451) 
        */
        static SunriseOrange(width: number = 1) {
            return new Pen(Color.SunriseOrange(), width);
        }

        /**
         * Mango Orange (#FF8040) 
        */
        static MangoOrange(width: number = 1) {
            return new Pen(Color.MangoOrange(), width);
        }

        /**
         * Dark Orange (#F88017) 
        */
        static DarkOrange(width: number = 1) {
            return new Pen(Color.DarkOrange(), width);
        }

        /**
         * Coral (#FF7F50) 
        */
        static Coral(width: number = 1) {
            return new Pen(Color.Coral(), width);
        }

        /**
         * Basket Ball Orange (#F88158) 
        */
        static BasketBallOrange(width: number = 1) {
            return new Pen(Color.BasketBallOrange(), width);
        }

        /**
         * Light Salmon (#F9966B) 
        */
        static LightSalmon(width: number = 1) {
            return new Pen(Color.LightSalmon(), width);
        }

        /**
         * Tangerine (#E78A61) 
        */
        static Tangerine(width: number = 1) {
            return new Pen(Color.Tangerine(), width);
        }

        /**
         * Dark Salmon (#E18B6B) 
        */
        static DarkSalmon(width: number = 1) {
            return new Pen(Color.DarkSalmon(), width);
        }

        /**
         * Light Coral (#E77471) 
        */
        static LightCoral(width: number = 1) {
            return new Pen(Color.LightCoral(), width);
        }

        /**
         * Bean Red (#F75D59) 
        */
        static BeanRed(width: number = 1) {
            return new Pen(Color.BeanRed(), width);
        }

        /**
         * Valentine Red (#E55451) 
        */
        static ValentineRed(width: number = 1) {
            return new Pen(Color.ValentineRed(), width);
        }

        /**
         * Shocking Orange (#E55B3C) 
        */
        static ShockingOrange(width: number = 1) {
            return new Pen(Color.ShockingOrange(), width);
        }

        /**
         * Red (#FF0000) 
        */
        static Red(width: number = 1) {
            return new Pen(Color.Red(), width);
        }

        /**
         * Scarlet (#FF2400) 
        */
        static Scarlet(width: number = 1) {
            return new Pen(Color.Scarlet(), width);
        }

        /**
         * Ruby Red (#F62217) 
        */
        static RubyRed(width: number = 1) {
            return new Pen(Color.RubyRed(), width);
        }

        /**
         * Ferrari Red (#F70D1A) 
        */
        static FerrariRed(width: number = 1) {
            return new Pen(Color.FerrariRed(), width);
        }

        /**
         * Fire Engine Red (#F62817) 
        */
        static FireEngineRed(width: number = 1) {
            return new Pen(Color.FireEngineRed(), width);
        }

        /**
         * Lava Red (#E42217) 
        */
        static LavaRed(width: number = 1) {
            return new Pen(Color.LavaRed(), width);
        }

        /**
         * Love Red (#E41B17) 
        */
        static LoveRed(width: number = 1) {
            return new Pen(Color.LoveRed(), width);
        }

        /**
         * Grapefruit (#DC381F) 
        */
        static Grapefruit(width: number = 1) {
            return new Pen(Color.Grapefruit(), width);
        }

        /**
         * Chestnut Red (#C34A2C) 
        */
        static ChestnutRed(width: number = 1) {
            return new Pen(Color.ChestnutRed(), width);
        }

        /**
         * Cherry Red (#C24641) 
        */
        static CherryRed(width: number = 1) {
            return new Pen(Color.CherryRed(), width);
        }

        /**
         * Mahogany (#C04000) 
        */
        static Mahogany(width: number = 1) {
            return new Pen(Color.Mahogany(), width);
        }

        /**
         * Chilli Pepper (#C11B17) 
        */
        static ChilliPepper(width: number = 1) {
            return new Pen(Color.ChilliPepper(), width);
        }

        /**
         * Cranberry (#9F000F) 
        */
        static Cranberry(width: number = 1) {
            return new Pen(Color.Cranberry(), width);
        }

        /**
         * Red Wine (#990012) 
        */
        static RedWine(width: number = 1) {
            return new Pen(Color.RedWine(), width);
        }

        /**
         * Burgundy (#8C001A) 
        */
        static Burgundy(width: number = 1) {
            return new Pen(Color.Burgundy(), width);
        }

        /**
         * Chestnut (#954535) 
        */
        static Chestnut(width: number = 1) {
            return new Pen(Color.Chestnut(), width);
        }

        /**
         * Blood Red (#7E3517) 
        */
        static BloodRed(width: number = 1) {
            return new Pen(Color.BloodRed(), width);
        }

        /**
         * Sienna (#8A4117) 
        */
        static Sienna(width: number = 1) {
            return new Pen(Color.Sienna(), width);
        }

        /**
         * Sangria (#7E3817) 
        */
        static Sangria(width: number = 1) {
            return new Pen(Color.Sangria(), width);
        }

        /**
         * Firebrick (#800517) 
        */
        static Firebrick(width: number = 1) {
            return new Pen(Color.Firebrick(), width);
        }

        /**
         * Maroon (#810541) 
        */
        static Maroon(width: number = 1) {
            return new Pen(Color.Maroon(), width);
        }

        /**
         * Plum Pie (#7D0541) 
        */
        static PlumPie(width: number = 1) {
            return new Pen(Color.PlumPie(), width);
        }

        /**
         * Velvet Maroon (#7E354D) 
        */
        static VelvetMaroon(width: number = 1) {
            return new Pen(Color.VelvetMaroon(), width);
        }

        /**
         * Plum Velvet (#7D0552) 
        */
        static PlumVelvet(width: number = 1) {
            return new Pen(Color.PlumVelvet(), width);
        }

        /**
         * Rosy Finch (#7F4E52) 
        */
        static RosyFinch(width: number = 1) {
            return new Pen(Color.RosyFinch(), width);
        }

        /**
         * Puce (#7F5A58) 
        */
        static Puce(width: number = 1) {
            return new Pen(Color.Puce(), width);
        }

        /**
         * Dull Purple (#7F525D) 
        */
        static DullPurple(width: number = 1) {
            return new Pen(Color.DullPurple(), width);
        }

        /**
         * Rosy Brown (#B38481) 
        */
        static RosyBrown(width: number = 1) {
            return new Pen(Color.RosyBrown(), width);
        }

        /**
         * Khaki Rose (#C5908E) 
        */
        static KhakiRose(width: number = 1) {
            return new Pen(Color.KhakiRose(), width);
        }

        /**
         * Pink Bow (#C48189) 
        */
        static PinkBow(width: number = 1) {
            return new Pen(Color.PinkBow(), width);
        }

        /**
         * Lipstick Pink (#C48793) 
        */
        static LipstickPink(width: number = 1) {
            return new Pen(Color.LipstickPink(), width);
        }

        /**
         * Rose (#E8ADAA) 
        */
        static Rose(width: number = 1) {
            return new Pen(Color.Rose(), width);
        }

        /**
         * Rose Gold (#ECC5C0) 
        */
        static RoseGold(width: number = 1) {
            return new Pen(Color.RoseGold(), width);
        }

        /**
         * Desert Sand (#EDC9AF) 
        */
        static DesertSand(width: number = 1) {
            return new Pen(Color.DesertSand(), width);
        }

        /**
         * Pig Pink (#FDD7E4) 
        */
        static PigPink(width: number = 1) {
            return new Pen(Color.PigPink(), width);
        }

        /**
         * Cotton Candy (#FCDFFF) 
        */
        static CottonCandy(width: number = 1) {
            return new Pen(Color.CottonCandy(), width);
        }

        /**
         * Pink Bubble Gum (#FFDFDD) 
        */
        static PinkBubbleGum(width: number = 1) {
            return new Pen(Color.PinkBubbleGum(), width);
        }

        /**
         * Misty Rose (#FBBBB9) 
        */
        static MistyRose(width: number = 1) {
            return new Pen(Color.MistyRose(), width);
        }

        /**
         * Pink (#FAAFBE) 
        */
        static Pink(width: number = 1) {
            return new Pen(Color.Pink(), width);
        }

        /**
         * Light Pink (#FAAFBA) 
        */
        static LightPink(width: number = 1) {
            return new Pen(Color.LightPink(), width);
        }

        /**
         * Flamingo Pink (#F9A7B0) 
        */
        static FlamingoPink(width: number = 1) {
            return new Pen(Color.FlamingoPink(), width);
        }

        /**
         * Pink Rose (#E7A1B0) 
        */
        static PinkRose(width: number = 1) {
            return new Pen(Color.PinkRose(), width);
        }

        /**
         * Pink Daisy (#E799A3) 
        */
        static PinkDaisy(width: number = 1) {
            return new Pen(Color.PinkDaisy(), width);
        }

        /**
         * Cadillac Pink (#E38AAE) 
        */
        static CadillacPink(width: number = 1) {
            return new Pen(Color.CadillacPink(), width);
        }

        /**
         * Carnation Pink (#F778A1) 
        */
        static CarnationPink(width: number = 1) {
            return new Pen(Color.CarnationPink(), width);
        }

        /**
         * Blush Red (#E56E94) 
        */
        static BlushRed(width: number = 1) {
            return new Pen(Color.BlushRed(), width);
        }

        /**
         * Hot Pink (#F660AB) 
        */
        static HotPink(width: number = 1) {
            return new Pen(Color.HotPink(), width);
        }

        /**
         * Watermelon Pink (#FC6C85) 
        */
        static WatermelonPink(width: number = 1) {
            return new Pen(Color.WatermelonPink(), width);
        }

        /**
         * Violet Red (#F6358A) 
        */
        static VioletRed(width: number = 1) {
            return new Pen(Color.VioletRed(), width);
        }

        /**
         * Deep Pink (#F52887) 
        */
        static DeepPink(width: number = 1) {
            return new Pen(Color.DeepPink(), width);
        }

        /**
         * Pink Cupcake (#E45E9D) 
        */
        static PinkCupcake(width: number = 1) {
            return new Pen(Color.PinkCupcake(), width);
        }

        /**
         * Pink Lemonade (#E4287C) 
        */
        static PinkLemonade(width: number = 1) {
            return new Pen(Color.PinkLemonade(), width);
        }

        /**
         * Neon Pink (#F535AA) 
        */
        static NeonPink(width: number = 1) {
            return new Pen(Color.NeonPink(), width);
        }

        /**
         * Magenta (#FF00FF) 
        */
        static Magenta(width: number = 1) {
            return new Pen(Color.Magenta(), width);
        }

        /**
         * Dimorphotheca Magenta (#E3319D) 
        */
        static DimorphothecaMagenta(width: number = 1) {
            return new Pen(Color.DimorphothecaMagenta(), width);
        }

        /**
         * Bright Neon Pink (#F433FF) 
        */
        static BrightNeonPink(width: number = 1) {
            return new Pen(Color.BrightNeonPink(), width);
        }

        /**
         * Pale Violet Red (#D16587) 
        */
        static PaleVioletRed(width: number = 1) {
            return new Pen(Color.PaleVioletRed(), width);
        }

        /**
         * Tulip Pink (#C25A7C) 
        */
        static TulipPink(width: number = 1) {
            return new Pen(Color.TulipPink(), width);
        }

        /**
         * Medium Violet Red (#CA226B) 
        */
        static MediumVioletRed(width: number = 1) {
            return new Pen(Color.MediumVioletRed(), width);
        }

        /**
         * Rogue Pink (#C12869) 
        */
        static RoguePink(width: number = 1) {
            return new Pen(Color.RoguePink(), width);
        }

        /**
         * Burnt Pink (#C12267) 
        */
        static BurntPink(width: number = 1) {
            return new Pen(Color.BurntPink(), width);
        }

        /**
         * Bashful Pink (#C25283) 
        */
        static BashfulPink(width: number = 1) {
            return new Pen(Color.BashfulPink(), width);
        }

        /**
         * Dark Carnation Pink (#C12283) 
        */
        static DarkCarnationPink(width: number = 1) {
            return new Pen(Color.DarkCarnationPink(), width);
        }

        /**
         * Plum (#B93B8F) 
        */
        static Plum(width: number = 1) {
            return new Pen(Color.Plum(), width);
        }

        /**
         * Viola Purple (#7E587E) 
        */
        static ViolaPurple(width: number = 1) {
            return new Pen(Color.ViolaPurple(), width);
        }

        /**
         * Purple Iris (#571B7E) 
        */
        static PurpleIris(width: number = 1) {
            return new Pen(Color.PurpleIris(), width);
        }

        /**
         * Plum Purple (#583759) 
        */
        static PlumPurple(width: number = 1) {
            return new Pen(Color.PlumPurple(), width);
        }

        /**
         * Indigo (#4B0082) 
        */
        static Indigo(width: number = 1) {
            return new Pen(Color.Indigo(), width);
        }

        /**
         * Purple Monster (#461B7E) 
        */
        static PurpleMonster(width: number = 1) {
            return new Pen(Color.PurpleMonster(), width);
        }

        /**
         * Purple Haze (#4E387E) 
        */
        static PurpleHaze(width: number = 1) {
            return new Pen(Color.PurpleHaze(), width);
        }

        /**
         * Eggplant (#614051) 
        */
        static Eggplant(width: number = 1) {
            return new Pen(Color.Eggplant(), width);
        }

        /**
         * Grape (#5E5A80) 
        */
        static Grape(width: number = 1) {
            return new Pen(Color.Grape(), width);
        }

        /**
         * Purple Jam (#6A287E) 
        */
        static PurpleJam(width: number = 1) {
            return new Pen(Color.PurpleJam(), width);
        }

        /**
         * Dark Orchid (#7D1B7E) 
        */
        static DarkOrchid(width: number = 1) {
            return new Pen(Color.DarkOrchid(), width);
        }

        /**
         * Purple Flower (#A74AC7) 
        */
        static PurpleFlower(width: number = 1) {
            return new Pen(Color.PurpleFlower(), width);
        }

        /**
         * Medium Orchid (#B048B5) 
        */
        static MediumOrchid(width: number = 1) {
            return new Pen(Color.MediumOrchid(), width);
        }

        /**
         * Purple Amethyst (#6C2DC7) 
        */
        static PurpleAmethyst(width: number = 1) {
            return new Pen(Color.PurpleAmethyst(), width);
        }

        /**
         * Dark Violet (#842DCE) 
        */
        static DarkViolet(width: number = 1) {
            return new Pen(Color.DarkViolet(), width);
        }

        /**
         * Violet (#8D38C9) 
        */
        static Violet(width: number = 1) {
            return new Pen(Color.Violet(), width);
        }

        /**
         * Purple Sage Bush (#7A5DC7) 
        */
        static PurpleSageBush(width: number = 1) {
            return new Pen(Color.PurpleSageBush(), width);
        }

        /**
         * Lovely Purple (#7F38EC) 
        */
        static LovelyPurple(width: number = 1) {
            return new Pen(Color.LovelyPurple(), width);
        }

        /**
         * Purple (#8E35EF) 
        */
        static Purple(width: number = 1) {
            return new Pen(Color.Purple(), width);
        }

        /**
         * Aztech Purple (#893BFF) 
        */
        static AztechPurple(width: number = 1) {
            return new Pen(Color.AztechPurple(), width);
        }

        /**
         * Medium Purple (#8467D7) 
        */
        static MediumPurple(width: number = 1) {
            return new Pen(Color.MediumPurple(), width);
        }

        /**
         * Jasmine Purple (#A23BEC) 
        */
        static JasminePurple(width: number = 1) {
            return new Pen(Color.JasminePurple(), width);
        }

        /**
         * Purple Daffodil (#B041FF) 
        */
        static PurpleDaffodil(width: number = 1) {
            return new Pen(Color.PurpleDaffodil(), width);
        }

        /**
         * Tyrian Purple (#C45AEC) 
        */
        static TyrianPurple(width: number = 1) {
            return new Pen(Color.TyrianPurple(), width);
        }

        /**
         * Crocus Purple (#9172EC) 
        */
        static CrocusPurple(width: number = 1) {
            return new Pen(Color.CrocusPurple(), width);
        }

        /**
         * Purple Mimosa (#9E7BFF) 
        */
        static PurpleMimosa(width: number = 1) {
            return new Pen(Color.PurpleMimosa(), width);
        }

        /**
         * Heliotrope Purple (#D462FF) 
        */
        static HeliotropePurple(width: number = 1) {
            return new Pen(Color.HeliotropePurple(), width);
        }

        /**
         * Crimson (#E238EC) 
        */
        static Crimson(width: number = 1) {
            return new Pen(Color.Crimson(), width);
        }

        /**
         * Purple Dragon (#C38EC7) 
        */
        static PurpleDragon(width: number = 1) {
            return new Pen(Color.PurpleDragon(), width);
        }

        /**
         * Lilac (#C8A2C8) 
        */
        static Lilac(width: number = 1) {
            return new Pen(Color.Lilac(), width);
        }

        /**
         * Blush Pink (#E6A9EC) 
        */
        static BlushPink(width: number = 1) {
            return new Pen(Color.BlushPink(), width);
        }

        /**
         * Mauve (#E0B0FF) 
        */
        static Mauve(width: number = 1) {
            return new Pen(Color.Mauve(), width);
        }

        /**
         * Wisteria Purple (#C6AEC7) 
        */
        static WisteriaPurple(width: number = 1) {
            return new Pen(Color.WisteriaPurple(), width);
        }

        /**
         * Blossom Pink (#F9B7FF) 
        */
        static BlossomPink(width: number = 1) {
            return new Pen(Color.BlossomPink(), width);
        }

        /**
         * Thistle (#D2B9D3) 
        */
        static Thistle(width: number = 1) {
            return new Pen(Color.Thistle(), width);
        }

        /**
         * Periwinkle (#E9CFEC) 
        */
        static Periwinkle(width: number = 1) {
            return new Pen(Color.Periwinkle(), width);
        }

        /**
         * Lavender Pinocchio (#EBDDE2) 
        */
        static LavenderPinocchio(width: number = 1) {
            return new Pen(Color.LavenderPinocchio(), width);
        }

        /**
         * Lavender blue (#E3E4FA) 
        */
        static Lavenderblue(width: number = 1) {
            return new Pen(Color.Lavenderblue(), width);
        }

        /**
         * Pearl (#FDEEF4) 
        */
        static Pearl(width: number = 1) {
            return new Pen(Color.Pearl(), width);
        }

        /**
         * SeaShell (#FFF5EE) 
        */
        static SeaShell(width: number = 1) {
            return new Pen(Color.SeaShell(), width);
        }

        /**
         * Milk White (#FEFCFF) 
        */
        static MilkWhite(width: number = 1) {
            return new Pen(Color.MilkWhite(), width);
        }

        /**
         * White (#FFFFFF) 
        */
        static White(width: number = 1) {
            return new Pen(Color.White(), width);
        }

        //endregion
    }
}