# Automatic deployment to runasp.net

The GitHub Actions workflow at `.github/workflows/deploy.yml` builds and
publishes this project whenever a commit is pushed to `main`, then uploads the
publish output to the hosting account over FTPS.

## Configure GitHub repository secrets

Open the repository's **Settings → Secrets and variables → Actions** and add:

| Secret | Value |
| --- | --- |
| `RUNASP_FTP_SERVER` | FTP/FTPS server hostname from the hosting control panel |
| `RUNASP_FTP_USERNAME` | FTP account username |
| `RUNASP_FTP_PASSWORD` | FTP account password |
| `RUNASP_FTP_SERVER_DIR` | Website application directory, including its trailing slash |
| `RUNASP_FTP_PROTOCOL` | Optional: `ftps` (default), `ftp`, or `ftps-legacy`, as supported by the host |

Use the application root directory expected by runasp.net, not a local project
directory. Do not put credentials in the workflow, source files, or commit
history.

After adding the secrets, push a commit to `main` and check **Actions** in
GitHub. The workflow preserves the remote `students.db` and its SQLite journal
files while deploying the published application.

The hosting plan must support the app's .NET 10 target. If the host only accepts
another runtime version or FTP protocol, adjust the target framework or
`RUNASP_FTP_PROTOCOL` to match the hosting settings.
