namespace MarketGame
{
    public static class DialogueManager
    {
        public static readonly string[] BuyTips = 
        [
            "Come down to the pub, there's been some kind of raid? The people are selling good {} for CHEAP.",
            "There's some commotion at the docks? I think someone is trying to undercut the mob on {}.",
            "Some guys are doing a HUGE sale on {}. Come quick!"
        ];

        public static readonly string[] SellTips =
        [
            "Idk what you have, but there is some SERIOUS demand for {} in the city! Hurry!",
            "Still got any {}? Some interested parties are willing to pay extra for it.",
            "There's a festival in town soon. I heard some people are gonna pay extra for {}."
        ];

        public static readonly string[] BustStrings =
        [
            "PUT YOUR HANDS IN THE AIR!",
            "Suspect tried to trade, close in...",
            "Its over, put your hands up!"
        ];

        public static readonly string[] PanicTextStrings =
        [
            "Hope you didn't buy any meth from the south side... turned out it was actual glass."
        ];
    }
}
