# Miner

[FAQ](FAQ.md)

```This is WIP and not ready for use```...

## For Developers

### Project Setup 

 - Clone this repro
 - Clone [HardlyCommon](https://github.com/hardlydifficult/HardlyCommon), [HardlyCommonWindows](https://github.com/hardlydifficult/HardlyCommonWindows), [Hardly's AutoUpdater.NET](https://github.com/hardlydifficult/AutoUpdater.NET) and [Hardly's XMR](https://github.com/hardlydifficult/xmr-stak-cpu) in the same directory (so Miner, HardlyCommon, HardlyCommondWindows, and xmr-stak-cpu folders are side by side)
 - If references are broken:
    - Open menu Tools -> NuGet Package Manager
    - Run ```Update-Package -reinstall```

### Sync your forked repository

Do this frequently if you are making changes. Each project will need occasional syncing.

 - Change directory to your local repository.
 - Switch to master branch if you are not ```git checkout master```
 - Add the parent as a remote repository, ```git remote add upstream https://github.com/hardlydifficult/Miner.git```
 - Issue ```git fetch upstream```
 - Issue ```git rebase upstream/master```
 - Check for pending merges with ```git status```
 - Issue ```git push origin master```
 
https://stackoverflow.com/a/31836086

## Building

For each build:

 - Increment the build version
 - Using Orca, Microsoft's MSI editing tool. Open the MSI file in Orca, select View-->Summary Information... then check the "UAC Compliant" checkbox.
 - To publish (HD only): Update the AutoUpdate.xml & push the MSI

From:  https://stackoverflow.com/questions/4080131/how-to-make-a-setup-work-for-limited-non-admin-users
