# Google Assistant Demo - CSharp

## Goal

This project is an adaptation in CSharp of the original code of [Nicolas Mauti](https://github.com/mautini/google-assistant-java-demo) in java.
This project was built and tested on a Mac. It should be working fine on others configurations.

## Configuration of Google Assistant Service

  * Go to https://console.cloud.google.com/.
  * Click to "Select a project" and choose "New project", enter the name of your project.
  * Copy and keep the Project ID, just under the Project Name field.
  * Select your new project and select API and Services
  * Choose Enable APIs and Services
  * Search Google Assistant API
  * Select the service and click on "Enable"
  * On the API and Services, click on Credentials
  * Select OAuth consent screen
  * Fill the different field and Save
  * Select Credentials and click on "Create Credentials"
  * Select OAuth client ID
  * Choose "Other", enter a name and click "Create"
  * Copy your ID and Secret Key
  * You should have now a project id, client id, and a client secret

## Build Project

Open the file [resources/reference.conf](https://github.com/wolfviking0/google-assistant-csharp-demo/blob/master/resources/reference.conf), replace *CLIENT_ID*, *CLIENT_SECRET* and *PROJECT_ID* with your info generated on the previous step

## Run Project

Open the solution [google-assistant-csharp-demo.sln](https://github.com/wolfviking0/google-assistant-csharp-demo/blob/master/google-assistant-csharp-demo.sln), just press the play button.
The first time the demo will open the browser in order to allow your device.
Connect with your google email, then allow/authorise your project.
After that just enter your request and press enter.

