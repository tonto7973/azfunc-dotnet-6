# Azure Functions V4 with Access Token Authentication (preview)

_Sample project that shows how to enable JWT access token authentication and authorization in Azure Function V4 project._

## Pre-requisites

- [Visual Studio 2022](https://docs.microsoft.com/en-us/visualstudio/releases/2022/release-notes-preview) (preview) with net6.0
- [NodeJS](https://nodejs.org/en/download/) with npm
- [Postman](https://www.postman.com/downloads/)

## Run the project

- Install Azurite - `npm install -g azurite`,
- Open `test-func-6.sln` in Visual Studio 2022,
- Run the solution (F5),
- Head over to https://okta-oidc-fun.herokuapp.com/ and
  1. Ensure you have `response_type` set to `token`,
  2. Click the link at the bottom of the page,
  3. On the next page copy the `access_token`.
- Use Postman and make the following HTTP request (replace `{access_token}` with the actual access token obtained in step 3. above):
  ```
  GET /api/HttpExample HTTP/1.1
  Host: localhost:7071
  Authorization: Bearer {access_token}
  ```

## Disclaimer

This is just an example. Use at your own risk. Ensure you have suitable unit tests and QA when productionising this code.

