# cync
cync is a portable cloud synchronization utility

# Why cync?
Tired of using heavy GUI applications to synchronize files with OneDrive or Google Drive? Then cync provides an alternative solution.

Looking for a tool which allows more efficient space use (compression) or provides additional security (encryption) for cloud storage? Then cync provides an alternative solution.

Likely to use various storage providers (Google Drive, OneDrive, sftp, local, etc) for backup? Then cync provides an alternative solution.

Using different operating systems and preferring tools which work on all? Then cync provides an alternative solution.

# Status
cync is in early alpha stages. Currently it's mostly useful to potential contributors of the project.

# Quick Introduction
cync has different operational modes, but let's start with the default example. Compression and security are enabled.

## Create the Repository
By default cync adds compression and better security for data stored in the cloud. To enable that, the first step is to create the repository:

    cync init --google-drive /onefolder --key-file c:/ttt/key-file.txt
    
The above command initliazes the remote repository in the **onefolder** folder. The provided key is used for encryption.

## Upload a folder
Once the repository is initialized, we can upload some content:

    cync push taxes /
    
The above commands synchronizes the local folder named taxes, with the /taxes folder in the repository. Since there is no such folder in the repository, the operation is an upload.

## Take a look at the content
Now I can list the files in the repository:

    cync list /
    
Can a use a GUI instead? NO! Here is what you see in the browser when you open one of **/onefolder** sub folders:

![Google Drive file list](https://github.com/ivannp/cync/blob/master/github-images/google-drive-files.png)

Remember, cync is encrypting by default. Not only the file content, but also the file system structure. Impossible to tell what's stored without using cync.

Once the folder is stored in the cloud, I can remove it from local drive.

In a year from now, when it's time to file taxes for 2019, here is the process to follow:

    # Download the remote /taxes folder to the current folder
    cync pull /taxes
    
    # Work on the taxes for 2019, adding files and folders
    
    # Push the changes to the cloud - uploads only the updated content
    cync push /taxes

More information is available in the porject Wiki.
