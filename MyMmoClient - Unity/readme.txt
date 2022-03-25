#Unity Play [Manual] Deploy 
In order to publish to unity play with current UnityVersion, package com.unity.connect.share aka "WebGL publish" is used
To initiate build, in UnityEditor click on "Publish" toolbar menu, and follow the instruction by selecting
a build folder that will not be added to source control, unity will generate two files, connectwebgl.zip
that contains all the build files generated, and after uploading that build, *I SUPPOSE* it uses webgl_sharing file
to identify entry on the play.unity.com so the new build is published to the same unity play url, e.g as if new version
NOTES:
-For unity play, in the PlayerSettings->WebGL Settings->Publishing Settings, option Compression Format can be set to gzip

#GitHub Pages [Semi Auto] Deploy, aks "Static Web Server"
For git hub pages, do regular Build of WebGL, specify an output folder or copy paste the output to
 
1.The Root of special git repo, commit, and publish updates if github repo is already setup,
- or setup GitHubPages, by creating empty git hub repo, then in the WebGL build output init new git repo,
add all the files, set the remote origin to newly created github repo, and publish.
- in the GitHub repo Settings, enable github pages, and select root folder and main branch as the source.
(in this case git game project can be private, but git game build should be public)

2.The <this project git root>docs/ and commit and publish changes, or if not GitHub Pages is not setup,
-go to settings and enable github pages by specifying source as main branch and docs/ subfolder.
here is the link for troubleshooting: https://alexandreasen09.medium.com/hosting-a-unity-webgl-game-for-free-f69ec70bcb30  
(in this case git game project and /docs folder will be public in public repo) 

NOTES:
-for GitHub pages, currently, PlayerSettings->WebGL Settings->Publishing Settings, option Compression Format should be disabled,
or compression fallback enabled