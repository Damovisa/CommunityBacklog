CommunityBacklog
================

Example of an application that integrates with Visual Studio Online using OAuth and REST APIs

This demo app has the following features:
* Built on the [NancyFX](http://nancyfx.org) framework
* SignalR for live vote counts
* Azure Table Storage for storage of potential backlog entries and OAuth keys
* [OAuth authentication](https://www.visualstudio.com/integrate/get-started/get-started-auth-oauth2-vsi?WT.mc_id=devops-0000-dabrady) against Visual Studio Online
* Use of a few [Visual Studio Online REST APIs](https://www.visualstudio.com/integrate/reference/reference-vso-overview-vsi?WT.mc_id=devops-0000-dabrady)

If you have any questions, hit me up on Twitter at @damovisa.

### Known Issue:

Currently there's no user management. The only credentials you have are stored in a cookie and consist of a VSO OAuth token. Unfortunately, when the "create a work item" request is made, this access token has likely expired, meaning we have to retrieve another one. This has the side-effect of invalidating the cookie we've stored locally in your browser.

I'll fix this by handling users better and keeping an up-to-date VSO OAuth token, however in the meantime you'll probably have to delete the cookie if you're going to use it more than once.
