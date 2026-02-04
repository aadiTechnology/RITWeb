# RITeSchool - School Management System

A comprehensive ASP.NET Web Forms application for managing school operations including admissions, student records, fee management, payroll, transport, and academic progress tracking.

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- **.NET Framework 4.8** or later
- **Visual Studio 2019** or later (recommended: Visual Studio 2022)
- **SQL Server** (Express, Developer, or Standard edition)
- **IIS** (for local development) or **IIS Express** (included with Visual Studio)
- **Git** (for version control)

## 🏗️ Project Structure

```
School/
├── BusinessLogic/          # Business logic layer
├── DataCommunicator/       # Data access layer
├── SchoolEntities/         # Entity Framework models
├── SchoolWebApp/           # Main web application project
│   ├── App_Data/          # Code-behind classes and data schemas
│   ├── App_GlobalResources/ # Global resources
│   ├── User Contol/       # User controls (.ascx)
│   └── Properties/        # Assembly info and project properties
├── School_Website/         # Website project (ASPX pages)
├── Utility/               # Utility classes and helpers
└── School.sln             # Visual Studio solution file
```

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd "Full Code Setup"
```

### 2. Restore NuGet Packages

Open the solution in Visual Studio and restore NuGet packages:

**Option A: Using Visual Studio**
1. Open `School.sln` in Visual Studio
2. Right-click the solution in Solution Explorer
3. Select **"Restore NuGet Packages"**
4. Wait for packages to restore

**Option B: Using Package Manager Console**
```powershell
# In Visual Studio, open Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
Update-Package -Reinstall
```

**Option C: Using Command Line**
```powershell
# Navigate to solution directory
cd "C:\path\to\Full Code Setup"

# Restore packages (if using MSBuild)
nuget restore School.sln
```

### 3. Configure Database Connection

1. Open `SchoolWebApp/Web.config`
2. Update the connection string in the `<connectionStrings>` section:

```xml
<connectionStrings>
  <add name="ApplicationServices" 
       connectionString="data source=YOUR_SERVER;Initial Catalog=SchoolDB;Integrated Security=True;" 
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

**⚠️ Security Note:** Never commit connection strings with production credentials. Use:
- `Web.config` for development (with local SQL Server)
- `Web.Release.config` transforms for deployment
- Environment variables or Azure Key Vault for production

### 4. Build the Solution

**In Visual Studio:**
1. Press `Ctrl+Shift+B` or go to **Build > Build Solution**
2. Ensure all projects build successfully

**Using Command Line:**
```powershell
msbuild School.sln /t:Build /p:Configuration=Debug
```

### 5. Run the Application

**Option A: Using Visual Studio**
1. Set `SchoolWebApp` or `School_Website` as the startup project (right-click project > Set as Startup Project)
2. Press `F5` or click **Start Debugging**
3. The application will launch in your default browser

**Option B: Using IIS Express (Command Line)**
```powershell
# Navigate to web project directory
cd SchoolWebApp

# Start IIS Express
"C:\Program Files\IIS Express\iisexpress.exe" /path:. /port:8080
```

**Option C: Using Local IIS**
1. Create a new application in IIS Manager
2. Point it to the `SchoolWebApp` or `School_Website` folder
3. Configure the application pool to use .NET Framework 4.8
4. Browse to the application URL

## 📦 NuGet Packages

The project uses the following NuGet packages (automatically restored):

- **Newtonsoft.Json** (v13.0.4) - JSON serialization
- **Microsoft.AspNet.Providers** - ASP.NET membership providers
- **System.Linq.Dynamic** - Dynamic LINQ queries

All packages are defined in `packages.config` files and will be automatically restored when you open the solution.

## 🔧 Development Workflow

### Adding New Files

1. Add new `.aspx` files to `School_Website` folder
2. Add code-behind classes to `SchoolWebApp/App_Data` or appropriate namespace
3. Add business logic to `BusinessLogic` project
4. Add data access code to `DataCommunicator` project

### Building and Testing

```powershell
# Clean solution
msbuild School.sln /t:Clean

# Build in Debug mode
msbuild School.sln /t:Build /p:Configuration=Debug

# Build in Release mode
msbuild School.sln /t:Build /p:Configuration=Release
```

## 🚫 Common Mistakes to Avoid

### ❌ Don't Commit These:

- **Binaries**: `bin/`, `obj/` folders
- **User-specific files**: `*.user`, `*.suo`, `.vs/`
- **NuGet packages**: `packages/` folder (use restore instead)
- **Build artifacts**: `*.dll`, `*.pdb` (except in specific scenarios)
- **SourceSafe files**: `*.scc`, `*.vssscc` (legacy VSS files)
- **Secrets**: Connection strings with passwords, API keys
- **Cache files**: `*.cache`, `*.log`

### ✅ Always Commit These:

- **Source code**: `*.cs`, `*.aspx`, `*.ascx`, `*.master`
- **Project files**: `*.csproj`, `*.sln`
- **Configuration templates**: `Web.config`, `Web.Debug.config`, `Web.Release.config`
- **Package references**: `packages.config`
- **Resources**: `*.resx`, `*.designer.cs`
- **Content**: `Scripts/`, `Content/`, `wwwroot/` (if applicable)

## 🔐 Security Best Practices

1. **Never commit secrets**: Use `Web.Release.config` transforms or environment variables
2. **Review connection strings**: Ensure no production credentials in `Web.config`
3. **Use transforms**: Leverage `Web.Debug.config` and `Web.Release.config` for environment-specific settings
4. **Sanitize logs**: Don't commit log files that may contain sensitive information

## 📝 Git Workflow

### Initial Setup (First Time)

```bash
# Initialize repository (if not already initialized)
git init

# Add .gitignore
git add .gitignore

# Stage all files (respecting .gitignore)
git add .

# Review what will be committed
git status

# Commit
git commit -m "Initial commit: ASP.NET School Management System"

# Add remote repository
git remote add origin <your-github-repo-url>

# Push to GitHub
git push -u origin main
```

### Daily Workflow

```bash
# Pull latest changes
git pull origin main

# Make your changes...

# Check status
git status

# Stage changes
git add .

# Commit
git commit -m "Description of changes"

# Push
git push origin main
```

## 🐛 Troubleshooting

### Issue: "Could not load file or assembly" errors

**Solution:** Ensure NuGet packages are restored:
```powershell
Update-Package -Reinstall
```

### Issue: Build errors after cloning

**Solution:** 
1. Clean solution: `Build > Clean Solution`
2. Restore NuGet packages
3. Rebuild: `Build > Rebuild Solution`

### Issue: Connection string errors

**Solution:** 
1. Verify SQL Server is running
2. Update connection string in `Web.config`
3. Ensure database exists or create it

### Issue: Missing references

**Solution:**
1. Check that all projects in solution are loaded
2. Verify project references are correct
3. Restore NuGet packages

## 📚 Additional Resources

- [ASP.NET Web Forms Documentation](https://docs.microsoft.com/en-us/aspnet/web-forms/)
- [.NET Framework 4.8 Documentation](https://docs.microsoft.com/en-us/dotnet/framework/)
- [NuGet Package Restore](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore)

## 👥 Contributing

1. Create a feature branch: `git checkout -b feature/your-feature-name`
2. Make your changes
3. Commit: `git commit -m "Add your feature"`
4. Push: `git push origin feature/your-feature-name`
5. Create a Pull Request

## 📄 License

[Specify your license here]

## 👤 Author

[Your Name/Organization]

---

**Note:** This project uses ASP.NET Web Forms (.aspx) and .NET Framework 4.8. For new projects, consider ASP.NET Core MVC or Razor Pages for better cross-platform support and modern development practices.
