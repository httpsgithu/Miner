
# Intro

The Hardly Difficult Miner is an application which will mine blockchain while your computer is idle.  Every few minutes while the miner is running, a small deposit is made. The money you make from mining goes directly to your favorite streamer, and you can set up your own Bitcoin wallet to keep a certain percent for yourself.


Currently working on implementing the [cryptonight algorithm](cryptonight.md) (see for progress).  

<details><summary>Why are You Doing This?</summary>

Streamers, like myself, depend on generous support from viewers in order to make a living. This program offers just another way of helping your favorite streamers make it.  It may be particularly compelling for those that are not able to financially support today, this is like watching ads for bits... but it simply runs in the background.

<hr></details><details><summary>What is Blockchain?</summary>

A blockchain is a trusted history.  A set of events, commonly transactions, are grouped together into a block.  That block is secured by miners and attached to the previous block, forming a chain all the way back to the very first transaction.

<hr></details><details><summary>What is Mining?</summary>

Mining is the process where the miner confirms a block is valid and then spends significant computer resources in order to secure it.  This process involves calculating a hash code that meets specific criteria.  The only way to achieve this is by guess and check, making the required hash code something which is difficult (i.e. slow) to calculate but easy (fast) to confirm.

<hr></details><details><summary>How do Miners Get Paid?</summary>

Normally, miners are paid only when they successfully add a new block to the chain.  Only one miner can do this at a time, which means every other miner working on that block has wasted their time, getting nothing in return unless they submit first.  This is commonly referred to as 'winning the lottery'.

We are using the NiceHash marketplace which pays for the work you do, not for winning the lottery.  This creates a consistent payout effectively by the hour.  Miners are buying computer time from people like you, hoping to win the lottery.

<hr></details><details><summary>What Exactly Is My Computer Doing For This?</summary>

This mining application uses just a single algorithm at the moment.  It will consume as much CPU as you allow it (defaulting at 1 thread).  There is a little bandwidth consumed but no memory used in the process.

<hr></details>

# More About Cryptocurrency

<details><summary>Is Bitcoin Private?</summary>

No.  Bitcoin is a history of transactions.  In order to confirm someone's account standing, you can walk the history to make sure they have the money they claim.  This means that the history is open for anyone to view.  

There are cryptocurrencies looking to create more private options.

<hr></details>

# Profitability

<details><summary>How much can you realistically make mining without specialized hardware?</summary>

My computer generates almost $10 / month at the moment, mining with just the CPU.

There are many different algorithms used in blockchain mining.  Some run well on GPU, others on specialized hardware (ASICS) specific to mining, and the one that this program is using runs best on a CPU (the [CryptoNight algorithm](https://en.bitcoin.it/wiki/CryptoNight)).

Here is a [calculator from NiceHash](https://www.nicehash.com/profitability-calculator), which includes estimated power cost.  

<hr></details><details><summary>Won't power cost more than you make?</summary>

Yea, but your mom pays for that.  Kappa.

For most, no.  Others, yes.  It depends on your hardware and where you live.  You can check the [calculator from NiceHash](https://www.nicehash.com/profitability-calculator) (note this app uses CPU only) if you know how much you pay for power.  For my machine/where I live, power costs about 10% of the earnings.

This application aims to limit how much power is consumed.  You can choose the number of threads, e.g. if you limit this to 1 your machine will not heat up.

<hr></details><details><summary>But the Numbers Are So Small, Why Bother?</summary>

If a few join in, this can really add up quicker than you may think.  For example, let's assume that everyone running the miner keeps it at 1 thread so their systems are never over loaded and run the miner for about 12 hours (some will do much more, others less).

```
My machine (i7-4790k) running 1 thread (which is 12% total CPU) pays about $.11 / day.
If running 12 hours/day: $.055 / day (equivalent to watching one ad for bits)
Per month: $1.65 on average per user or $100/month for a streamer with 60 supporters.
```

If that same machine ran 100%, it would be about $0.33 / day or $9.90 / month.  This is more support for the streamer than a paid subscription.

<hr></details>

# About NiceHash

<details><summary>Why Bitcoin specifically, why not mine other currencies?</summary>

We are.  NiceHash pays in Bitcoin for the work completed, but that work may include mining any blockchain (e.g. Bitcoin or zCash).

<hr></details><details><summary>Is This a "Pool"?</summary>

No, NiceHash is a marketplace, not a standard "pool".  The difference is NiceHash will pay for the work that you do, while a pool distributes winnings when someone in the pool wins the lottery.


<hr></details><details><summary>How Do I Get Paid?</summary>

There is no sign-up required for NiceHash.  All you need is a Bitcoin wallet address.  I recommend Coinbase.com.

If you are mining for your own wallet, or you are a streamer others are mining for, every few seconds a tiny deposit is made into a NiceHash account shown on the [dashboard](https://www.nicehash.com/miner/14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE) (just replace the end with your wallet).

Once you reach the minimum, it transfers from the NiceHash account to your wallet.  This happens once per day, if you don't meet the minimum yet it carries over to the next until you do.

If you use a NiceHash wallet (instead of something like coinbase), then there is another minimum to reach and the only withdrawal option is to transfer it to an external wallet such as coinbase.

<hr></details><details><summary>Is There a Minimum to Make a Withdrawal?</summary>

Yes.  About $60 if you use a standard bitcoin wallet, ~$15 with a NiceHash wallet.  The transaction fees work out in your favor if you use an external wallet, such as coinbase... but hitting that minimum may take some time.

Every transaction along the way includes a transaction fee.  

To go from coinbase to cash, you can:

 - Deposit to a Bank Account for 1.49%.
 - Deposit to Paypal for 3.49%. 

<hr></details>



# Other

<details><summary>Why CPU Only?</summary>

We could add GPU support if there is interest in this.  It adds a fair bit of complexity, in order to do this well.

Mining with the CPU only is safe.  If you keep the thread count down (it defaults to 1), it will not impact other things running - like a Twitch stream or the game your playing.  Using the GPU to mine may cause your machine to lag.

You may consider getting the NiceHash client instead, this supports GPU mining and may generate more revenue ($50-$100/mo for one computer with a good GPU is possible).  The downside is that it is a resource hog always running at 100%, not to be run while using the machine for anything else and power costs will be higher.

<hr></details><details><summary>Linux? Mac? Mobile?</summary>

It's possible, but not yet.

<hr></details><details><summary>I Stream, How Do I Sign Up?</summary>

If you're a Twitch affiliate, just let me know you're interested (others, see below).  We'll add you to a list of streamers that users can choose from... and create a download link which defaults to sending 99% of the profits to you, and the other 1% to me. 

I'm still working on a few core features, and the ui.  It'll be some time before it would be ready to ask the masses to install.

<hr></details><details><summary>I Don't Stream, How Do I Sign Up?</summary>

I'm hoping to set this up so that it's easily configurable.  If you wanted to used this as a fundraiser to go towards charity, for a group of friends, another company, whatever... Let's do it!  Basically I'll post instructions on how to reconfigure the settings so you can post a zip with your wallets instead of streamers.  

You can add one or many wallets. But, like with streamers, 1% will go to my wallet. Let me know if there's anything specific you need.  

<hr></details><details><summary>Is This Open Source?</summary>

Yes.

<hr></details><details><summary>What's Next?</summary>

If things continue to go well, after building this application most of it will be reused to create a Unity asset store package.  This could be a great alternative to watching ads as way to support developers of free-to-play games.

<hr></details>

<br><br>

Hardly Difficult's wallet: 14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE