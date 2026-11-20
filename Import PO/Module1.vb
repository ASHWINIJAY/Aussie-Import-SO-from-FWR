Imports System.Text
Imports System.IO

Module Module1
    'Public UserId As String = Nothing
    'declare a oledb connection to connect to the excel file  
    Public strQryMsg As String
    Dim MyConnection As System.Data.OleDb.OleDbConnection

    'declare a dataset  
    Dim DtSet As System.Data.DataSet
    'declare a oledb command  
    Public _sysUsername As String
    Public _sysPass As String
    Public _sysComp As String
    Public _sysCompPass As String
    Public SOResults As New List(Of String)
    Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
    Public _username As String
    Public Sub Main(Optional ByVal args As String() = Nothing)
        Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        ' MessageBox.Show(args.Length)
        If args.Length > 0 Then
            _username = args(0)
            ' MessageBox.Show(_username)
        End If
        Dim homefrm As New frmImportPo2()
        Application.Run(homefrm)
    End Sub

    Sub createsrt(ByVal objFile As Object)

        Dim ds As DataSet = Get_Excel("")

        Dim dr As DataRow = Nothing

        Dim dt As DataTable = ds.Tables(0)

        For Each dr In dt.Rows
            Console.WriteLine(dr)
        Next

        Dim ConnString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" & _
            CStr(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\ExcelToAccess.mdb")

        Dim DBCon As New OleDb.OleDbConnection(ConnString)

        DBCon.Open()

        Dim cm As New OleDb.OleDbCommand(" insert into Table1 (CustAcctNum) VALUES  ('" & dr.Item(0) & "')", DBCon)

        Try

            cm.ExecuteNonQuery()

            DBCon.Close()

        Catch ex As Exception
        Finally

        End Try

    End Sub

    Public Function Get_Excel(ByVal filePath As String)

        Try

            'give the connection its string value  

            MyConnection = New System.Data.OleDb.OleDbConnection _
                ("Provider=Microsoft.ACE.OLEDB.12.0; Data Source= " & filePath & "; Extended Properties=Excel 12.0")

            'give the command its SQL query  

            MyConnection.Open()

            MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection)

            '*** need the values in an array???  

            DtSet = New System.Data.DataSet

            MyCommand.Fill(DtSet)

            MyConnection.Close()

        Catch ex As Exception

            MsgBox(ex.Message & vbNewLine & vbNewLine & " Please Check File Path and Try Again.")

            Return Nothing
        End Try
        Return DtSet
    End Function

    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

        'Dim Contents As String
        GC.Collect()
        Dim XMLfile As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            XMLfile = True
        Catch Ex As Exception
            MsgBox("Error creating xml file " & Ex.Message, MsgBoxStyle.Exclamation, "")
            ErrInfo = Ex.Message
        End Try
        Return XMLfile

    End Function

End Module

