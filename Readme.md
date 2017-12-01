# Miner

[FAQ](FAQ.md)

```This is WIP and not ready for use```...

## For Developers

### Project Setup 

 - Clone this repro
 - Run SyncDependencies.bat
   - This will clone and/or sync [HardlyCommon](https://github.com/hardlydifficult/HardlyCommon), [HardlyCommonWindows](https://github.com/hardlydifficult/HardlyCommonWindows), [Hardly's AutoUpdater.NET](https://github.com/hardlydifficult/AutoUpdater.NET) and [Hardly's XMR](https://github.com/hardlydifficult/xmr-stak-cpu)
   - If references are broken (but they should be fine):
      - Open menu Tools -> NuGet Package Manager
      - Run ```Update-Package -reinstall```
 - (Optional) Install [Setup Project support](https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects)
    - Otherwise simply Remove the project from the solution (please don't check in the change to the solution)
 - Set the startup project to Miner.App.UI.WPF
 - (Optional) Build xmr-stak-cpu
    - We checked in the built dlls, you can remove the xmr projects if you do not want to look at c++ ;)
 - Run
  

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

 - Increment the versions
   - Miner -> Properties (right click menu) -> Application -> Assembly Information -> Assembly Version
   - Miner Installer -> Properties (the panel) ->  Version
 - Rebuild both Miner and MinerSetup
 - (HD only) To Publish:
   - Run Deploy.bat (updates UAC permissions and moves the MSI for publishing and prompts for version)
   - Push the site

From:  https://stackoverflow.com/questions/4080131/how-to-make-a-setup-work-for-limited-non-admin-users
