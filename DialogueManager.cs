using System.Printing;

namespace MarketGame
{
    public static class DialogueManager
    {
        // TODO: Rewrite these
        public static readonly string[] BuyTips =
        [
            "Come down to the pub, there's been some kind of raid? The people are selling good {} for CHEAP.",
            "There's some commotion at the docks? I think someone is trying to undercut the mob on {}.",
            "Some guys are doing a HUGE sale on {}. Come quick!",
            "Down at the pub, there's a guy selling hot {} after a raid. He's letting them go for cheap to get rid of the evidence.",
            "Heard there's a ruckus at the docks. Looks like someone's undercutting the mob on {}. You might score a deal if you're quick.",
            "A bunch of guys are offloading a stash of {} at a massive discount. Sounds like they’re trying to fence the goods fast.",
            "A recent heist left a surplus of {}. They're selling it off for cheap before anyone gets suspicious.",
            "There's been a big bust, and now someone’s looking to dump their {} stock at rock-bottom prices.",
            "A new player in town is making waves by selling {} way below the usual rates. Might be a good time to stock up.",
            "Just got a tip: someone's pushing stolen {} in the back alleys. Prices are too good to pass up.",
            "An inside source says there's a fire sale on {} at the old warehouse. They're moving the goods fast before the heat catches on.",
            "At the underground market, they're practically giving away {}. Looks like they're trying to offload some hot merchandise.",
            "The latest shipment of {} fell off the back of a truck. You can grab them for a steal if you act fast.",
            "A smuggler’s unloading a batch of {} at the dockside market. They're pricing it to move quickly.",
            "A crooked cop is clearing out evidence, selling {} for dirt cheap. It's a once-in-a-lifetime deal.",
            "A rival gang is liquidating their stock of {} to fund a turf war. Prices are low, but the risks are high.",
            "Someone’s unloading counterfeit {}. They look real enough, and the price is unbeatable.",
            "The feds are sniffing around, and a supplier is dumping {} at a huge discount to lay low."
        ];

        // TODO: Rewrite these
        public static readonly string[] SellTips =
        [
            "Idk what you have, but there is some SERIOUS demand for {} in the city! Hurry!",
            "Still got any {}? Some interested parties are willing to pay extra for it.",
            "There's a festival in town soon. I heard some people are gonna pay extra for {}.",
            "Word on the street is the syndicate's in need of {}. They’re paying top dollar—no questions asked.",
            "Got a tip: a high-roller in town is looking to buy up all the {} they can find. Might be your lucky day to cash in.",
            "The black market's heating up—demand for {} is through the roof. Now's the time to offload any you’ve got.",
            "Rumor has it the boss's kid is throwing a party and needs {}. They'll pay extra to get it quickly.",
            "A certain club in the east end is desperate for {}. If you still have any, you could make a tidy profit.",
            "The underground auction is happening soon. I hear {} is the hot item this time—get yours in the mix.",
            "There's a new gang moving into town, and they’re stocking up on {}. They’re offering good money to build connections.",
            "Some rich collector is putting out feelers for {}. They’re paying top prices to get their hands on it.",
            "The local fixer needs {} for a big job. You could make some serious cash by supplying it.",
            "A big shipment of {} just got intercepted, creating a shortage. Now's your chance to sell at a premium.",
            "A certain casino boss has a thing for {}. If you have some, they’re willing to pay over market value for the right quality.",
            "An upcoming underground event has created a buzz—people are scrambling to buy {}. Perfect time to sell.",
            "The cops are cracking down, and some folks are looking to stockpile {} before things get too hot. Sell while the market's hot!",
            "A well-connected informant says the cartel's looking for a new supplier of {}. They’re offering a sweet deal.",
            "A high-end buyer just put out word they need {} for a private gathering. They're paying a premium for discretion and speed."
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



        // NAMES
        public static readonly string[] SpamNames =
        [
            "Oceanic Vacations",
            "FastCash Investments",
            "SecureBank Alerts",
            "Survey4You",
            "LuxWatch Deals",
            "ShipNow Express",
            "SlimDown Solutions",
            "QuickLoan Pro",
            "HeartMatch",
            "SafePC Tools",
            "TaxPro Services",
            "CryptoGains",
            "VIP Shopper Club",
            "SuccessDaily News",
            "Heritage Trust"
        ];

        public static readonly string[] SpamTexts =
        [
            "Congratulations! You've won a free cruise! Click here to claim your prize now!",
            "Exclusive offer: Get rich quick! Just send $1000 to our special account, and receive $10,000 in a week!",
            "Urgent: Your bank account has been compromised! Please provide your details to verify your identity.",
            "You've been selected to participate in a top-secret survey! Complete it to win a brand new smartphone!",
            "HOT DEAL: Discounted luxury watches, up to 90% off! Limited time only!",
            "Your package is delayed! Click here to reschedule your delivery.",
            "Limited Offer: Buy one get one free on all miracle weight-loss pills! Transform your body now!",
            "Act fast! You've been pre-approved for a loan of up to $50,000. Apply now to secure your funds!",
            "Congratulations! You've been selected for a FREE trial of our exclusive dating app. Find your perfect match today!",
            "Is your computer slow? You may have viruses! Click here to download our free virus removal tool!",
            "Urgent message from the IRS: Please verify your tax information immediately to avoid penalties.",
            "Amazing opportunity! Invest in crypto now and double your money within a month!",
            "You've been chosen for a special VIP discount at our online store! Don't miss out on exclusive deals.",
            "Breaking news: Local billionaire reveals how to make $10,000 a day from home. Click to learn more!",
            "We found a lost relative who wants to leave you an inheritance. Please send us your details to proceed."
        ];

        public static readonly string[] ContactDescriptions =
        [
            // The Broker
            "Nothing is known about him, some people say he's a ghost. But in 2024 even ghosts have facebook profiles. All that is known is that he facilitates" +
            " a lot of the deals that go down in this town, and for some reason he's taken a liking to you. Do NOT fuck this up.",

            // Igor
            "Igor Petrovitch, the Russian mafia's finest enforcer. He loves nothing more than breaking knees and drinking vodka... god he loves vodka. He doesn't" + 
            " have much sway in this town but he does have money, and he is more than willing to pay you to outsource the work. It won't do good for your image..." +
            " or your criminal record but beggars can't be choosers. \n\nIgor also isn't going to win any beauty contests anytime soon.",

            // Officer smith
            "You know what real power looks like? Bent cops. Or informants? Its hard to tell with officer smith. With a name like James Smith we don't really think" +
            " he is the most loyal mob associate but that's what he claims. Any dealings with him stay strictly out of the gaze of your contemporaries, though, Smith" +
            " does not need to be seen as a cop. You could probably bribe him to lower the heat on you... or you could wait it out.\n\nSmith has shadowy motives, like" +
            " everyone else in this town, however, money is an excellent lubricant for your dealings with him.",
        
            // The Runner
            "Mother Theresa, you are not. The runner is some poor junkie you got off the street to do your dirty work. What's his name? You don't know! Truly, you are" +
            " a capitalist at heart. He will do just about anything for a fix, or for enough money to get his fix. Why risk your criminal record when you can get him to" +
            " do your hard time? If he gets arrested you will have to wait, but he will be right back, in the brutal cycle that the streets are. What a shame." +
            "\n\nAll that being said... you will not get the respect or recognition when he does something cool, so don't expect to bask in his glory.",

            // Student Union
            "You are a hardened criminal who dodges bullets and sells to other hardened criminals and you do hardened criminal things... \n\n" +
            " And the local university! They do more than just study there. The students won't pay nearly as much but it is fast cash and you have no right to complain" +
            " about that right now. The police certainly won't care, but your peers might. Don't expect to be renowned for selling drugs to students at a discount."
        ];

        public static readonly string[] CharacterTaglines =
        [
            "A shadowy figure who quite likes you.",
            "Enforcer for the Russian mafia. Vodka sommelier.",
            "Your favourite bent cop... or informer. We don't know.",
            "A junkie, plucked off the street to do your bidding. Now don't you feel evil?",
            "A student union with its priorities set. AAA : Acid, Alcohol, and Academia"
        ];
    }
}
