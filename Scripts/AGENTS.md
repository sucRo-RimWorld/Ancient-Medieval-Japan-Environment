# Scripts agent instructions — AMJ Environment

Read the repository-root `AGENTS.md`, `main:Docs/Coordination.md`, and the Project shared rules first. The following constraints apply to PowerShell development here:

- PowerShell changes must not be committed directly to `main`; stage `.ps1` edits on a topic branch and open a pull request.
- Keep `.github/workflows/powershell-syntax.yml` as the repository-level parser gate and require the triggered PowerShell syntax check to pass before merging.
- `run-tests.bat` must execute `Scripts/Validate-PowerShellSyntax.ps1` before build/static validation so local test runs fail fast on parser errors.
- Avoid large string-replacement edits to `Validate-Environment.ps1`; after a focused edit, re-read both the edited block and end of file to catch truncation or duplicated tails.
