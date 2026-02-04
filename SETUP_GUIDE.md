# Git Setup Guide for ASP.NET Project

This guide provides step-by-step instructions for initializing your Git repository and pushing your ASP.NET solution to GitHub.

## 📁 Expected Folder Structure After Cleanup

After applying `.gitignore`, your repository should contain:

```
School/
├── .git/                          # Git repository (hidden)
├── .gitignore                     # ✅ Git ignore rules
├── README.md                      # ✅ Project documentation
├── SETUP_GUIDE.md                 # ✅ This file
│
├── BusinessLogic/                 # ✅ Source code
│   ├── *.cs
│   ├── BusinessLogic.csproj
│   └── packages.config
│
├── DataCommunicator/              # ✅ Source code
│   ├── *.cs
│   ├── DataCommunicator.csproj
│   └── packages.config
│
├── SchoolEntities/                 # ✅ Source code
│   ├── *.cs
│   ├── SchoolEntities.csproj
│   └── packages.config
│
├── SchoolWebApp/                  # ✅ Web application
│   ├── App_Data/                  # ✅ Code-behind classes
│   ├── App_GlobalResources/       # ✅ Resources
│   ├── User Contol/               # ✅ User controls
│   ├── Properties/                # ✅ Assembly info
│   ├── *.aspx                     # ✅ Web pages
│   ├── *.ascx                     # ✅ User controls
│   ├── Web.config                 # ✅ Configuration
│   ├── Web.Debug.config           # ✅ Debug transform
│   ├── Web.Release.config         # ✅ Release transform
│   ├── SchoolWebApp.csproj        # ✅ Project file
│   └── packages.config            # ✅ NuGet packages
│
├── School_Website/                # ✅ Website project
│   └── RITeSchool/
│       └── *.aspx                 # ✅ Web pages
│
├── Utility/                       # ✅ Utility classes
│   ├── *.cs
│   ├── Utility.csproj
│   └── packages.config
│
└── School.sln                     # ✅ Solution file
```

### ❌ Excluded Folders/Files (Not in Git)

```
School/
├── .vs/                           # ❌ Visual Studio cache
├── packages/                      # ❌ NuGet packages (restored via restore)
├── */bin/                         # ❌ Compiled binaries
├── */obj/                         # ❌ Build artifacts
├── *.user                         # ❌ User-specific settings
├── *.suo                          # ❌ Solution user options
├── *.scc                          # ❌ SourceSafe files
├── *.cache                        # ❌ Cache files
└── *.log                          # ❌ Log files
```

## 🚀 Step-by-Step Git Setup

### Step 1: Verify Current State

```powershell
# Navigate to your project directory
cd "C:\Users\lenovo\AadiTech\RITeSchool\Sachin\Full Code Setup"

# Check if Git is already initialized
git status
```

**If you see:** `fatal: not a git repository` → Proceed to Step 2  
**If you see:** Git status output → Skip to Step 3

### Step 2: Initialize Git Repository

```powershell
# Initialize a new Git repository
git init

# Verify initialization
git status
```

### Step 3: Add .gitignore

```powershell
# Ensure .gitignore exists (it should be in the root directory)
# Verify it's there
dir .gitignore

# Add .gitignore to staging
git add .gitignore

# Commit .gitignore first
git commit -m "Add .gitignore for ASP.NET project"
```

### Step 4: Review What Will Be Committed

```powershell
# Check what files Git will track (respecting .gitignore)
git status

# See detailed list of files to be added
git add --dry-run .

# If you want to see what will be ignored
git status --ignored
```

**Expected output:** You should see:
- ✅ `.gitignore`
- ✅ `README.md`
- ✅ `SETUP_GUIDE.md`
- ✅ `School.sln`
- ✅ All `*.csproj` files
- ✅ All `*.aspx` files
- ✅ All `*.cs` files
- ✅ `Web.config` files
- ✅ `packages.config` files
- ❌ No `bin/` or `obj/` folders
- ❌ No `packages/` folder
- ❌ No `*.user` files

### Step 5: Stage All Files

```powershell
# Stage all files (respecting .gitignore)
git add .

# Verify what's staged
git status
```

### Step 6: Create Initial Commit

```powershell
# Commit all staged files
git commit -m "Initial commit: ASP.NET School Management System

- Add solution and project files
- Add source code (C#, ASPX, ASCX)
- Add configuration files
- Add documentation (README, SETUP_GUIDE)
- Exclude build artifacts, binaries, and user-specific files"
```

### Step 7: Create GitHub Repository

1. Go to [GitHub.com](https://github.com)
2. Click **"New repository"** (or the **+** icon)
3. Repository name: `RITeSchool` (or your preferred name)
4. Description: `ASP.NET School Management System`
5. **Visibility:** Choose Public or Private
6. **DO NOT** initialize with README, .gitignore, or license (we already have these)
7. Click **"Create repository"**

### Step 8: Connect Local Repository to GitHub

```powershell
# Add remote repository (replace YOUR_USERNAME and REPO_NAME)
git remote add origin https://github.com/YOUR_USERNAME/REPO_NAME.git

# Verify remote was added
git remote -v
```

**Example:**
```powershell
git remote add origin https://github.com/yourusername/RITeSchool.git
```

### Step 9: Push to GitHub

```powershell
# Rename branch to 'main' if needed (GitHub default)
git branch -M main

# Push to GitHub
git push -u origin main
```

**If prompted for credentials:**
- Use your GitHub username and a **Personal Access Token** (not password)
- Create token: GitHub → Settings → Developer settings → Personal access tokens → Generate new token
- Select scopes: `repo` (full control of private repositories)

### Step 10: Verify Push

1. Go to your GitHub repository page
2. Verify all files are present
3. Check that `bin/`, `obj/`, `packages/` folders are **NOT** visible
4. Verify `.gitignore` is present

## 🔄 Daily Workflow

### Pull Latest Changes

```powershell
git pull origin main
```

### Make Changes and Commit

```powershell
# Check status
git status

# Stage specific files
git add path/to/file.cs

# Or stage all changes
git add .

# Commit
git commit -m "Description of your changes"

# Push
git push origin main
```

### Create a Feature Branch

```powershell
# Create and switch to new branch
git checkout -b feature/add-new-feature

# Make changes, commit
git add .
git commit -m "Add new feature"

# Push branch to GitHub
git push -u origin feature/add-new-feature

# Create Pull Request on GitHub, then merge to main
```

## ✅ Verification Checklist

Before pushing, verify:

- [ ] `.gitignore` is in the root directory
- [ ] No `bin/` folders are tracked (check with `git ls-files | findstr bin`)
- [ ] No `obj/` folders are tracked
- [ ] No `packages/` folder is tracked
- [ ] No `*.user` files are tracked
- [ ] No `*.suo` files are tracked
- [ ] `Web.config` is tracked (but no secrets in it)
- [ ] All `*.csproj` files are tracked
- [ ] All `*.sln` files are tracked
- [ ] All source code files (`*.cs`, `*.aspx`) are tracked
- [ ] `packages.config` files are tracked

### Quick Verification Commands

```powershell
# Check for accidentally tracked binaries
git ls-files | findstr "\.dll$"
git ls-files | findstr "\.pdb$"

# Check for tracked user files
git ls-files | findstr "\.user$"
git ls-files | findstr "\.suo$"

# Check for tracked packages folder
git ls-files | findstr "packages\\"

# Should return nothing (or only expected files)
```

## 🐛 Troubleshooting

### Issue: "packages folder is being tracked"

**Solution:**
```powershell
# Remove from Git cache (but keep local files)
git rm -r --cached packages/

# Commit the removal
git commit -m "Remove packages folder from tracking"

# Push
git push origin main
```

### Issue: "bin/obj folders are being tracked"

**Solution:**
```powershell
# Remove from Git cache
git rm -r --cached */bin/
git rm -r --cached */obj/

# Commit
git commit -m "Remove build artifacts from tracking"

# Push
git push origin main
```

### Issue: "Large file size warning"

**Solution:**
```powershell
# Check repository size
git count-objects -vH

# If packages or binaries were committed, remove them (see above)
# Consider using Git LFS for large files if needed
```

### Issue: "Authentication failed"

**Solution:**
- Use Personal Access Token instead of password
- Or set up SSH keys for authentication

## 📊 Repository Size Expectations

**Expected size:** 10-50 MB (source code only)  
**Too large:** > 100 MB (likely includes binaries/packages)

If your repository is too large:
1. Check for tracked binaries: `git ls-files | findstr "\.dll$"`
2. Remove them from tracking (see Troubleshooting)
3. Consider using `git filter-branch` or BFG Repo-Cleaner for history cleanup

## 🎯 Best Practices

1. **Always review before committing:** `git status` and `git diff`
2. **Commit frequently:** Small, logical commits
3. **Write clear commit messages:** Describe what and why
4. **Never commit secrets:** Use config transforms or environment variables
5. **Keep .gitignore updated:** Add new patterns as needed
6. **Test after clone:** Verify `git clone → restore → build → run` works

## 📚 Additional Resources

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [.gitignore Templates](https://github.com/github/gitignore)
- [NuGet Package Restore](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore)

---

**Ready to push?** Follow Steps 1-10 above, and you'll have a clean, production-ready repository on GitHub! 🚀
