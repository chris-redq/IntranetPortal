Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection

Public Class SQLHelper
    Public Shared strConnect As String

    Public Sub New()

    End Sub

    Shared Sub New()
        strConnect = System.Configuration.ConfigurationManager.ConnectionStrings("WFServer").ConnectionString '"data source=CHRISPC;initial catalog=WFServer;integrated security=True;"
    End Sub

    Public Function GetConnection() As SqlConnection
        Dim oconn As SqlConnection = New SqlConnection(strConnect)
        Return oconn
    End Function

    Public Function ExecuteNonQuery(ByVal strSQL As String) As Integer
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQL, oconn)
        command.CommandType = CommandType.Text
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteNonQuery(ByVal strSQL As String, ByVal params() As SqlParameter) As Integer
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQL, oconn)
        command.CommandType = CommandType.Text
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteNonQuerySP(ByVal strSQLSP As String) As Integer
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteNonQuerySP(ByVal strSQLSP As String, ByVal params() As SqlParameter) As Integer
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteNonQueryCommand(ByVal strSQL As String, ByVal params() As SqlParameter) As SqlCommand
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQL, oconn)
        command.CommandType = CommandType.Text
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        Return command
    End Function

    Public Function ExecuteNonQueryCommandSP(ByVal strSQLSP As String, ByVal params() As SqlParameter) As SqlCommand
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Integer = command.ExecuteNonQuery()
        oconn.Close()
        oconn.Dispose()
        Return command
    End Function

    Public Function ExecuteScalar(ByVal strSQL As String) As Object
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQL, oconn)
        command.CommandType = CommandType.Text
        oconn.Open()
        Dim retval As Object = command.ExecuteScalar()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval

    End Function

    Public Function ExecuteScalar(ByVal strSQL As String, ByVal params() As SqlParameter) As Object
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQL, oconn)
        command.CommandType = CommandType.Text
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Object = command.ExecuteScalar()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteScalarSP(ByVal strSQLSP As String) As Object
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        oconn.Open()
        Dim retval As Object = command.ExecuteScalar()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function


    Public Function ExecuteReaderSP(ByVal strSQLSP As String, params() As SqlParameter) As SqlDataReader
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim dataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
        Return dataReader
    End Function

    Public Function ExecuteReaderSP(ByVal strSQLSP As String) As SqlDataReader
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        oconn.Open()
        Dim dataReader = command.ExecuteReader(CommandBehavior.CloseConnection)
        Return dataReader
    End Function

    Public Function ExecuteScalarSP(ByVal strSQLSP As String, ByVal params() As SqlParameter) As Object
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        For i As Integer = 0 To params.Length - 1
            command.Parameters.Add(params(i))
        Next
        oconn.Open()
        Dim retval As Object = command.ExecuteScalar()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return retval
    End Function

    Public Function ExecuteDataSet(ByVal strSQL As String) As DataSet
        Dim oconn As SqlConnection = GetConnection()
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter(strSQL, oconn)
        da.Fill(ds)
        oconn.Close()
        oconn.Dispose()
        da.Dispose()
        Return ds
    End Function

    Public Function ExecuteDataSet(ByVal strSQL As String, ByVal params() As SqlParameter) As DataSet
        Dim oconn As SqlConnection = GetConnection()
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter(strSQL, oconn)
        For i As Integer = 0 To params.Length - 1
            da.SelectCommand.Parameters.Add(params(i))
        Next
        da.Fill(ds)
        oconn.Close()
        oconn.Dispose()
        da.Dispose()
        Return ds
    End Function

    Public Function ExecuteDataSetSP(ByVal strSQLSP As String) As DataSet
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter(command)
        da.Fill(ds)
        da.Dispose()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return ds
    End Function

    Public Function ExecuteDataSetSP(ByVal strSQLSP As String, ByVal params() As SqlParameter) As DataSet
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter(command)
        For i As Integer = 0 To params.Length - 1
            da.SelectCommand.Parameters.Add(params(i))
        Next
        da.Fill(ds)
        da.Dispose()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return ds
    End Function


    Public Function ExecuteDataTable(ByVal strSQL As String) As DataTable
        Dim oconn As SqlConnection = GetConnection()
        Dim dt As New DataTable
        Dim da As New SqlDataAdapter(strSQL, oconn)
        da.Fill(dt)
        oconn.Close()
        oconn.Dispose()
        da.Dispose()
        Return dt
    End Function

    Public Function ExecuteDataTable(ByVal strSQL As String, ByVal params() As SqlParameter) As DataTable
        Dim oconn As SqlConnection = GetConnection()
        Dim dt As New DataTable
        Dim da As New SqlDataAdapter(strSQL, oconn)
        For i As Integer = 0 To params.Length - 1
            da.SelectCommand.Parameters.Add(params(i))
        Next
        da.Fill(dt)
        oconn.Close()
        oconn.Dispose()
        da.Dispose()
        Return dt
    End Function

    Public Function ExecuteDataTableSP(ByVal strSQLSP As String) As DataTable
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        Dim dt As New DataTable
        Dim da As New SqlDataAdapter(command)
        da.Fill(dt)
        da.Dispose()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return dt
    End Function

    Public Function ExecuteDataTableSP(ByVal strSQLSP As String, ByVal params() As SqlParameter) As DataTable
        Dim oconn As SqlConnection = GetConnection()
        Dim command As New SqlCommand(strSQLSP, oconn)
        command.CommandType = CommandType.StoredProcedure
        Dim dt As New DataTable
        Dim da As New SqlDataAdapter(command)
        For i As Integer = 0 To params.Length - 1
            da.SelectCommand.Parameters.Add(params(i))
        Next
        da.Fill(dt)
        da.Dispose()
        oconn.Close()
        oconn.Dispose()
        command.Dispose()
        Return dt
    End Function

    ''' <summary>
    ''' The Names Of The Columns Must Match The Names Of The Properties
    ''' </summary>
    ''' <param name="myObject"></param>
    ''' <param name="myData"></param>
    ''' <remarks></remarks>
    Public Sub PopulateObject(ByVal myObject As Object, ByVal myData As DataTable)
        For Each row As DataRow In myData.Rows
            With myObject
                For Each p As PropertyInfo In myObject.GetType().GetProperties()
                    Try
                        p.SetValue(myObject, row(p.Name), Nothing)
                    Catch ex As Exception

                    End Try
                Next
            End With
            Exit For
        Next
    End Sub

    ''' <summary>
    ''' The Names Of The Columns Must Match The Names Of The Properties
    ''' </summary>
    ''' <param name="myObject"></param>
    ''' <param name="myData"></param>
    ''' <remarks></remarks>
    Public Sub PopulateObject(ByVal myObject As Object, ByVal myData As DataRow)
        With myObject
            For Each p As PropertyInfo In myObject.GetType().GetProperties()
                Try
                    p.SetValue(myObject, myData(p.Name), Nothing)
                Catch ex As Exception

                End Try
            Next
        End With
    End Sub
End Class
