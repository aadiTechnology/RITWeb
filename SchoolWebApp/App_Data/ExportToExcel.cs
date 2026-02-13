using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

/// <summary>
/// Summary description for ExportToExcel
/// </summary>
public class ExportToExcel : SchoolBase
{
	public ExportToExcel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// This method is used to set format properties.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void AddFormatProperties(Worksheet aoWorksheet)
    {
        SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D };
        aoWorksheet.Append(sheetFormatProperties1);
    }

    /// <summary>
    /// This method is used to set sheet dimensions.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void AddSheedDimension(Worksheet aoWorksheet)
    {
        SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1:B55" };
        aoWorksheet.Append(sheetDimension1);
    }

    /// <summary>
    /// This method is used to set sheet properties.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void AddSheetProperties(Worksheet aoWorksheet)
    {
        SheetProperties sheetProperties1 = new SheetProperties();
        PageSetupProperties pageSetupProperties1 = new PageSetupProperties() { FitToPage = true };
        sheetProperties1.Append(pageSetupProperties1);
        aoWorksheet.Append(sheetProperties1);
    }


    /// <summary>
    /// This method is used to set workbook properties.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    protected void AddWorkbookProperties(Workbook aoWorkbook)
    {
        WorkbookProperties workbookProperties1 = new WorkbookProperties() { DefaultThemeVersion = (UInt32Value)124226U };
        aoWorkbook.Append(workbookProperties1);
    }

    /// <summary>
    /// This method is used to set file version.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    protected void AddFileVersion(Workbook aoWorkbook)
    {
        FileVersion fileVersion1 = new FileVersion() { ApplicationName = "xl", LastEdited = "4", LowestEdited = "4", BuildVersion = "4505" };
        aoWorkbook.Append(fileVersion1);
    }

    /// <summary>
    /// This method is sued to add sheet details.
    /// </summary>
    /// <param name="worksheet1"></param>
    protected void AddSheetDetails(Worksheet worksheet1)
    {
        AddSheetProperties(worksheet1);

        AddSheedDimension(worksheet1);

        AddSheetView(worksheet1);

        AddFormatProperties(worksheet1);
    }

    /// <summary>
    /// This method is used to set sheet view.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void AddSheetView(Worksheet aoWorksheet)
    {
        SheetViews sheetViews1 = new SheetViews();

        SheetView sheetView1 = new SheetView() { ShowGridLines = false, TabSelected = true, WorkbookViewId = (UInt32Value)0U };
        Selection selection1 = new Selection() { SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1:B1" } };

        sheetView1.Append(selection1);

        sheetViews1.Append(sheetView1);
        aoWorksheet.Append(sheetViews1);
    }

    /// <summary>
    /// This method is used to set book view.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    protected void AddBookViews(Workbook aoWorkbook)
    {
        BookViews bookViews1 = new BookViews();
        WorkbookView workbookView1 = new WorkbookView() { XWindow = 120, YWindow = 30, WindowWidth = (UInt32Value)20055U, WindowHeight = (UInt32Value)9990U };
        bookViews1.Append(workbookView1);
        aoWorkbook.Append(bookViews1);
    }


    /// <summary>
    /// This method is used to set calculation properties.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    protected void AddCalculationProperties(Workbook aoWorkbook)
    {
        CalculationProperties calculationProperties1 = new CalculationProperties() { CalculationId = (UInt32Value)124519U };
        aoWorkbook.Append(calculationProperties1);
    }

    /// <summary>
    /// This method is used to add sheet.
    /// </summary>
    /// <param name="aoWorkbook"></param>
    protected void AddSheets(Workbook aoWorkbook, string asSheetName)
    {
        Sheets sheets1 = new Sheets();
        Sheet sheet1 = new Sheet() { Name = asSheetName, SheetId = (UInt32Value)1U, Id = "rId1" };
        sheets1.Append(sheet1);
        aoWorkbook.Append(sheets1);
    }

    /// <summary>
    /// This method is used to generate part contents.
    /// </summary>
    /// <param name="aoPart"></param>
    protected void GeneratePartContent(WorkbookPart aoPart, string asSheetName)
    {
        Workbook workbook1 = new Workbook();
        workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        AddFileVersion(workbook1);

        AddWorkbookProperties(workbook1);

        AddBookViews(workbook1);

        AddSheets(workbook1, asSheetName);

        AddCalculationProperties(workbook1);

        aoPart.Workbook = workbook1;

    }
    
    /// <summary>
    /// This method is sued to add cell.
    /// </summary>
    /// <param name="asVal"></param>
    /// <param name="aoCellValues"></param>
    /// <param name="aoStypeIndex"></param>
    /// <returns></returns>
    protected Cell AddCell(string asVal, CellValues aoCellValues, StudentPaidFeeEnum aoStypeIndex)
    {
        return new Cell()
        {
            CellValue = new CellValue(asVal),
            DataType = new EnumValue<CellValues>(aoCellValues),
            StyleIndex = Convert.ToUInt16(aoStypeIndex)
        };

    }

    /// <summary>
    /// This method is used to set print options
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void AddPrintOptions(Worksheet aoWorksheet)
    {
        DocumentFormat.OpenXml.Spreadsheet.PrintOptions printOptions1 = new DocumentFormat.OpenXml.Spreadsheet.PrintOptions() { HorizontalCentered = true };
        aoWorksheet.Append(printOptions1);
    }

    /// <summary>
    /// This method is used to set page setup.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void SetPageSetup(Worksheet aoWorksheet, OrientationValues aoOrientationValues)
    {
        PageSetup pageSetup1 = new PageSetup() { PaperSize = (UInt32Value)8U, Orientation = aoOrientationValues, Id = "rId1", FitToHeight = (UInt32Value)0U };
        aoWorksheet.Append(pageSetup1);
    }

    /// <summary>
    /// This method is used to set margin.
    /// </summary>
    /// <param name="aoWorksheet"></param>
    protected void SetPageMargin(Worksheet aoWorksheet, double dbLeftMargin)
    {
        DocumentFormat.OpenXml.Spreadsheet.PageMargins pageMargins1 = new DocumentFormat.OpenXml.Spreadsheet.PageMargins() { Left = dbLeftMargin, Right = 0.25D, Top = 0.25D, Bottom = 0.50D, Header = 0.25D, Footer = 0.25D };
        aoWorksheet.Append(pageMargins1);
    }

    protected void GenerateReportStyles(WorkbookStylesPart aoWorkbookStylesPart1)
    {
        Fonts fonts1 = new Fonts(
           new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Bold { Val = true }
           ),
           new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" }
           ),
           new Font( // Index 0 - default
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF000000" },
               new Bold { Val = true }
           )
           ,
           new Font( // Index 0 - default
              new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
              new FontName { Val = "Arial" },
              new Color { Rgb = "FFFF0000" },
              new Bold { Val = true }
          ),
          new Font( // Index 0 - default
              new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 12 },
              new FontName { Val = "Arial" },
              new Bold { Val = true }
          ),
          new Font(
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF50C878" }        // green        
           ),
           new Font(
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FFE97451" } // 
           ),
            new Font(
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FF6495ED" } // 
           ),
           new Font(
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Color { Rgb = "FFFFC0CB" } // 
           ),
           new Font(
               new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 10 },
               new FontName { Val = "Arial" },
               new Bold { Val = true }
           )
         );

        Fills fills1 = new Fills(
               new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
               new Fill(new PatternFill() { PatternType = PatternValues.LightGray }), // Index 1 - default
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "A9A9A9" } }) { PatternType = PatternValues.Solid }), // Index 2 - header
               new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "E8E8E8" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
           );

        Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

        CellFormats cellFormats1 = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 90U) },
                new CellFormat { FontId = 0, FillId = 3, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 5, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 6, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 7, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Left, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 5, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 6, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 7, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 2, FillId = 0, BorderId = 0, ApplyBorder = false, Alignment = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center, 0U) },                
                new CellFormat { FontId = 0, FillId = 2, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Right, VerticalAlignmentValues.Center, 0U) },
                new CellFormat { FontId = 9, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = GetAlignment(HorizontalAlignmentValues.Center, VerticalAlignmentValues.Center, 0U) }
            );

        aoWorkbookStylesPart1.Stylesheet = new Stylesheet(fonts1, fills1, borders, cellFormats1); ;
    }

    /// <summary>
    /// This method is used to return alignment.
    /// </summary>
    /// <param name="aoHorizontalAlignment"></param>
    /// <param name="aoVerticalAlignment"></param>
    /// <returns></returns>
    protected DocumentFormat.OpenXml.Spreadsheet.Alignment GetAlignment(HorizontalAlignmentValues aoHorizontalAlignment, VerticalAlignmentValues aoVerticalAlignment, UInt32Value aiRotationValue)
    {
        DocumentFormat.OpenXml.Spreadsheet.Alignment alnCenterHeader = new DocumentFormat.OpenXml.Spreadsheet.Alignment
        {
            Vertical = aoVerticalAlignment,
            WrapText = true,
            Horizontal = aoHorizontalAlignment,
            TextRotation = aiRotationValue
        };

        if (aoHorizontalAlignment == HorizontalAlignmentValues.Left)
            alnCenterHeader.Indent = (UInt32Value)1U;

        return alnCenterHeader;
    }

    protected string GetReferenceName(int aiStartIndex)
    {
        string sCellStart;
        if (aiStartIndex >= 183)
            sCellStart = "F" + ((char)(64 + (aiStartIndex - 182))).ToString();
        else if (aiStartIndex >= 157)
            sCellStart = "F" + ((char)(64 + (aiStartIndex - 156))).ToString();
        else if (aiStartIndex >= 131)
            sCellStart = "E" + ((char)(64 + (aiStartIndex - 130))).ToString();
        else if (aiStartIndex >= 105)
            sCellStart = "D" + ((char)(64 + (aiStartIndex - 104))).ToString();
        else if (aiStartIndex >= 79)
            sCellStart = "C" + ((char)(64 + (aiStartIndex - 78))).ToString();
        else if (aiStartIndex >= 53)
            sCellStart = "B" + ((char)(64 + (aiStartIndex - 52))).ToString();
        else if (aiStartIndex >= 27)
            sCellStart = "A" + ((char)(64 + (aiStartIndex - 26))).ToString();
        else
            sCellStart = ((char)(64 + aiStartIndex)).ToString();

        return sCellStart;
    }
    
    public enum StudentPaidFeeEnum
    {
        LeftHeader = 1,
        CenterHeader = 2,
        LeftData = 3,
        CenterData = 4,
        NoBorderCenterHeader = 5,
        CenterHeader90 =6,
        CenterDataGray = 7,
        LeftDataWithGreenColor = 8,
        LeftDataWithLightRedColor = 9,
        LeftDataWithLightBlueColor = 10,
        CenterDataWithGreenColor = 8,
        CenterDataWithLightRedColor = 9,
        RightHeader = 15,
        RightData = 16        
    }

    public enum ExcelReportEnum
    {
        LeftHeader = 1,
        CenterHeader = 2,
        LeftData = 3,
        CenterData = 4,
        NoBorderCenterHeader = 5,
        CenterHeader90 = 6,
        CenterDataGray = 7,
        LeftDataWithGreenColor = 8,
        LeftDataWithLightRedColor = 9,
        LeftDataWithLightBlueColor = 10,
        CenterDataWithGreenColor = 8,
        CenterDataWithLightRedColor = 9,
        CenterDataWithLightBlueColor = 10
    }
}