Partial Public Class PendingAssignLead

    Public Shared Function AssignedBatch(batch As LeadsAssignedBatch) As String
        If batch Is Nothing OrElse batch.BBLEs Is Nothing OrElse batch.BBLEs.Count = 0 Then
            Throw New Exception("Batch is empty.")
        End If

        ' create the batch id
        Dim id = Guid.NewGuid().ToString

        Using ctx As New Entities
            For Each bbl In batch.BBLEs.Distinct
                Dim li = ctx.LeadsInfoes.Find(bbl)
                If (li Is Nothing) Then
                    li = New LeadsInfo
                    li.BBLE = bbl
                    li.CreateDate = DateTime.Now
                    li.CreateBy = batch.Assigned
                    li.LeadsTags = batch.Tags + ";"
                    ctx.LeadsInfoes.Add(li)
                Else
                    li.LeadsTags = li.LeadsTags + batch.Tags + ";"
                End If

                Dim pl = ctx.PendingAssignLeads.Find(bbl)
                If pl Is Nothing Then
                    pl = New PendingAssignLead
                    pl.BBLE = bbl

                    ctx.PendingAssignLeads.Add(pl)
                End If

                pl.CreateDate = DateTime.Now
                pl.CreateBy = id
                pl.EmployeeName = batch.Employee
                pl.Status = PendingStatus.Active
            Next

            ctx.SaveChanges()
        End Using

        Return id
    End Function

    Public Shared Function GetBatchStatus(id As String) As Object
        Using ctx As New Entities
            Dim result = ctx.PendingAssignLeads.Where(Function(a) a.CreateBy = id).ToList

            Return New With {
                    .Active = result.Where(Function(a) a.Status = PendingStatus.Active).Count,
                    .InProcess = result.Where(Function(a) a.Status = PendingStatus.InLoop).Count,
                    .Completed = result.Where(Function(a) a.Status = PendingStatus.Complete).Count,
                    .Detail = result
                }
        End Using
    End Function

    Public Shared Function GetAllPendingLeads() As List(Of PendingAssignLead)
        Using ctx As New Entities
            Return ctx.PendingAssignLeads.Where(Function(s) s.Status = PendingStatus.Active).ToList
        End Using
    End Function

    Public Shared Function GetInLoopLeads() As List(Of PendingAssignLead)
        Using ctx As New Entities
            Return ctx.PendingAssignLeads.Where(Function(s) s.Status = PendingStatus.InLoop).ToList
        End Using
    End Function

    'Public Sub Assign()
    '    'If ld Is Nothing Then
    '    '    ld = DataWCFService.UpdateAssessInfo(BBLE)
    '    'End If

    '    'DataWCFService.UpdateLeadInfo(BBLE, False, False, False, False, False, False, True)
    '    Dim ld = LeadsInfo.GetInstance(BBLE)
    '    ld.AssignTo(EmployeeName, CreateBy)
    '    Finish()
    'End Sub

    Public Sub Update(status As PendingStatus)
        Using ctx As New Entities
            Dim pLd = ctx.PendingAssignLeads.Find(BBLE)
            If pLd IsNot Nothing Then
                pLd.Status = status
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    Public Sub Finish()
        Using ctx As New Entities
            Dim pLd = ctx.PendingAssignLeads.Find(BBLE)
            If pLd IsNot Nothing Then
                pLd.Status = PendingStatus.Complete
                pLd.FinishDate = DateTime.Now
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    Enum PendingStatus
        Active
        InLoop
        Complete
    End Enum
End Class

Public Class LeadsAssignedBatch
    Public Property BBLEs As List(Of String)
    Public Property Tags As String
    Public Property Employee As String
    Public Property Assigned As String
End Class
