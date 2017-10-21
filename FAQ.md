
# Intro

The Hardly Difficult Miner is an application which will mine blockchain while your computer is idle.  Every few minutes while the miner is running, a small deposit is made. The money you make from mining goes directly to your favorite streamer, and you can set up your own Bitcoin wallet to keep a certain percent for yourself.

## Why are You Doing This?

Streamers, like myself, depend on generous support from viewers in order to make a living. This program offers just another way of helping your favorite streamers make it.  It may be particularly compelling for those that are not able to financially support today, this is like watching ads for bits... but it simply runs in the background.

## What is Blockchain?

A blockchain is a trusted history.  A set of events, commonly transactions, are grouped together into a block.  That block is secured by miners and attached to the previous block, forming a chain all the way back to the very first transaction.

## What is Mining?

Mining is the process where the miner confirms a block is valid and then spends significant computer resources in order to secure it.  This process involves calculating a hash code that meets specific criteria.  The only way to achieve this is by guess and check, making the require hash code something which is difficult (i.e. slow) to calculate but easy (i.e. fast) to confirm.

## How do Miners Get Paid?

Normally, miners are paid only when they successfully add a new block to the chain.  Only one miner can do this at a time, which means every other miner working on that block has wasted their time, getting nothing in return unless they submit first.  This is commonly referred to as 'winning the lottery'.

We are using the NiceHash marketplace which pays for the work you do, not for winning the lottery.  This creates a consistent payout effectively by the hour.

# More About Cryptocurrency

## Is Bitcoin Private?

No.  Bitcoin is a history of transactions.  In order to confirm someone's account standing, you can walk the history to make sure they have the money they claim.  This means that the history is open for anyone to view.  

There are cryptocurrencies looking to create more private options.

# Profitability

## How much can you realistically make mining without specialized hardware?

My computers generate an average of $0.33 / day at the moment.

There are many different algorithms used in blockchain mining.  Some run well on GPU, others on specialized hardware (ASICS) specific to mining, and the one that this program is using runs best on a CPU (the [CryptoNight algorithm](https://en.bitcoin.it/wiki/CryptoNight)).

Here is a [calculator from NiceHash](https://www.nicehash.com/profitability-calculator), which includes estimated power cost.  

You can also consider getting the NiceHash client instead, this supports GPU mining and may generate more revenue.  The downside is that it is a resource hog always running at 100%, definitely not to be run while using the machine for anything else.

## Won't power cost more than you make?

Yea, but your mom pays for that.  Kappa.

For some, yes.  Others, no.  It depends on your hardware and where you live.  You can check the [calculator from NiceHash](https://www.nicehash.com/profitability-calculator) if you know how much you pay for power.  

This application aims to limit how much power is consumed.  You can choose the number of threads, e.g. if you limit this to 1 your machine will not heat up.

## But the Numbers Are So Small, Why Bother?

If a few join in, this can really add up quicker than you may think.  For example, let's assume that everyone running the miner keeps it at 1 thread so their systems are never over loaded and run the miner for about 12 hours (some will do much more, others less).

```
My machine (i7-4790k) running 1 thread (which is 12% total CPU) pays about $.11 / day.
If running 12 hours/day: $.055 / day (equivalent to watching one ad for bits)
Per month: $1.65 on average per user or $100/month for a streamer with 60 supporters.
```

If that same machine ran 100%, it would be about $0.33 / day or $9.90 / month.  This is more support for the streamer than a paid subscription.

# About NiceHash

## Why Bitcoin specifically, why not mine other currencies?

We are.  NiceHash pays in Bitcoin for the work completed, but that work may include mining any blockchain (e.g. Bitcoin or zCash).