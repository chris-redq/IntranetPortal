Partial Public Class ProcessScheme

    Public Shared Function GetAllScheme() As List(Of ProcessScheme)
        Using ctx As New DataModelContainer
            Return ctx.ProcessSchemes.ToList
        End Using
    End Function

    Public Shared Function GetSchemeById(procId As Guid) As ProcessScheme
        Using ctx As New DataModelContainer
            Dim ps = ctx.ProcessSchemes.Find(procId)
            Return ps
        End Using
    End Function

    Public Shared Function GetProcessName(procId As Integer) As String
        Using ctx As New DataModelContainer
            Dim ps = ctx.ProcessSchemes.Find(procId)
            Return ps.Name
        End Using
    End Function

    Public Shared Function GetProcessDisplayName(procId As Integer) As String
        Using ctx As New DataModelContainer
            Dim ps = ctx.ProcessSchemes.Find(procId)
            Return ps.DisplayName
        End Using
    End Function

    Shared Function GetSchemeByName(processName As String) As ProcessScheme
        Using ctx As New DataModelContainer
            Return ctx.ProcessSchemes.SingleOrDefault(Function(ps) ps.Name = processName And ps.IsDefault = True)
        End Using
    End Function

    Public ReadOnly Property XScheme As XElement
        Get
            Return XElement.Parse(Scheme)
        End Get
    End Property

End Class
