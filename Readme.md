# Miner

```This is WIP and not ready for use```...

## For Developers

### Project Setup 

 - Clone this repro
 - Clone [HardlyCommon](https://github.com/hardlydifficult/HardlyCommon) and [Hardly's XMR](https://github.com/hardlydifficult/xmr-stak-cpu) in the same directory (so Miner, HardlyCommon, and xmr-stak-cpu folders are side by side)
 - If references are broken:
    - Open menu Tools -> NuGet Package Manager
    - Run ```Update-Package -reinstall```

### Sync your forked repository

**Do this frequently if you are making changes**

 - Change directory to your local repository.
 - Switch to master branch if you are not ```git checkout master```
 - Add the parent as a remote repository, ```git remote add upstream https://github.com/hardlydifficult/ChatBot.git```
 - Issue ```git fetch upstream```
 - Issue ```git rebase upstream/master```
 - Check for pending merges with ```git status```
 - Issue ```git push origin master```
 
https://stackoverflow.com/a/31836086
