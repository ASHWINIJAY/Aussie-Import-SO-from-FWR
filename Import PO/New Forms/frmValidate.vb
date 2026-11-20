Imports System.Drawing.Printing
Imports System.IO

Public Class frmValidate
    ' Inherits Form

    Private printPreviewDialog1 As New PrintPreviewDialog()
    Private WithEvents printDocument1 As New PrintDocument()

    ' Declare a string to hold the entire document contents.
    Private documentContents As String

    ' Declare a variable to hold the portion of the document that
    ' is not printed.
    Private stringToPrint As String

    Private Sub ReadDocument()
        Try
            Dim docName As String = "ValidateErrs.txt"
            Dim docPath As String = "c:\Temp\"
            printDocument1.DocumentName = docName
            Dim stream As New FileStream(docPath + docName, FileMode.Open)
            Try
                Dim reader As New StreamReader(stream)
                Try
                    documentContents = reader.ReadToEnd()
                Finally
                    reader.Dispose()
                End Try
            Finally
                stream.Dispose()
            End Try
            stringToPrint = documentContents

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Sub printDocument1_PrintPage(ByVal sender As Object, _
        ByVal e As PrintPageEventArgs) Handles printDocument1.PrintPage

        Dim charactersOnPage As Integer = 0
        Dim linesPerPage As Integer = 0

        ' Sets the value of charactersOnPage to the number of characters 
        ' of stringToPrint that will fit within the bounds of the page.
        e.Graphics.MeasureString(stringToPrint, Me.Font, e.MarginBounds.Size, _
            StringFormat.GenericTypographic, charactersOnPage, linesPerPage)

        ' Draws the string within the bounds of the page.
        e.Graphics.DrawString(stringToPrint, Me.Font, Brushes.Black, _
            e.MarginBounds, StringFormat.GenericTypographic)

        ' Remove the portion of the string that has been printed.
        stringToPrint = stringToPrint.Substring(charactersOnPage)

        ' Check to see if more pages are to be printed.
        e.HasMorePages = stringToPrint.Length > 0

        ' If there are no more pages, reset the string to be printed.
        If Not e.HasMorePages Then
            stringToPrint = documentContents
        End If

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        ReadDocument()
        printPreviewDialog1.Document = printDocument1
        printPreviewDialog1.ShowDialog()
    End Sub

End Class