
Public Class RemoteLeadsQuery

    ''' <summary>
    ''' Return the remote query result
    ''' </summary>
    ''' <param name="query">sql script</param>
    ''' <returns>list of bbles</returns>
    Public Shared Function QueryResult(query As String) As List(Of String)
        Using ctx As New Entities
            Dim result = ctx.RemoteLeadsQuery(query)
            Return result.ToList
        End Using
    End Function

End Class
