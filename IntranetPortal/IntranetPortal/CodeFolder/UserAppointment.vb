﻿Partial Public Class UserAppointment
    Public Sub NewAppointment()
        Using context As New Entities
            CreateBy = HttpContext.Current.User.Identity.Name
            CreateDate = DateTime.Now

            context.UserAppointments.Add(Me)
            context.SaveChanges()
        End Using

    End Sub

    Public Shared Function UpdateAppointmentStatus(logId As Integer, status As AppointmentStatus) As UserAppointment

        Using Context As New Entities
            Dim userAppoint = Context.UserAppointments.Where(Function(t) t.LogID = logId).SingleOrDefault

            If userAppoint IsNot Nothing Then
                userAppoint.Status = status
                Context.SaveChanges()
            End If

            Return userAppoint
        End Using
    End Function

    Public Shared Sub ExpiredAppointmentByBBLE(bble As String)
        Using Context As New Entities
            Dim userAppoint = Context.UserAppointments.Where(Function(t) t.BBLE = bble).ToList

            For Each apoit In userAppoint
                apoit.Status = AppointmentStatus.Expired
            Next

            Context.SaveChanges()
        End Using
    End Sub

    Public Shared Function GetMyTodayAppointments(name As String) As List(Of UserAppointment)
        Using Context As New Entities

            Dim appoints = (From appoint In Context.UserAppointments
                           Where appoint.Status = UserAppointment.AppointmentStatus.Accepted And (appoint.Agent = name Or appoint.Manager = name)
                           Select appoint).Distinct.ToList.Where(Function(api) api.ScheduleDate.Value.Date = DateTime.Today).ToList

            Return appoints
        End Using
    End Function

    Public Shared Function GetAppointmentBylogID(logId As Integer) As UserAppointment

        Using context As New Entities
            Return context.UserAppointments.Where(Function(ua) ua.LogID = logId).SingleOrDefault
        End Using

    End Function

    Enum AppointmentStatus
        NewAppointment
        Accepted
        Declined
        ReScheduled
        Expired
    End Enum
End Class
