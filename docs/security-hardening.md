# Security Hardening Notes

## Configuration

- keep all checked-in secrets placeholder-based
- prefer environment variables or local-only overrides for real values
- review Docker files and sample appsettings before every public push

## Operations

- avoid logging sensitive headers, tokens, and connection strings
- make health and error behavior clear without exposing private values
- treat dev examples as examples only, not deployable defaults

## Repository Hygiene

- scan the working tree before pushing
- purge leaks from history, not just from the latest commit
- review copied notes, scripts, and support files with the same care as code
