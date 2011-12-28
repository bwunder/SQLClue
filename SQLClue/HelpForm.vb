Public Class HelpForm

    Public Overloads Function Show(ByVal HelpPage As String) As DialogResult

        If HelpPage Is Nothing OrElse HelpPage = "" Then
            HelpPage = "SQLClue.htm"
        End If
        'HelpPagesWebBrowser.DocumentStream = File.OpenRead(My.Application.Info.DirectoryPath & "\Help\" & HelpPage)
        HelpPagesWebBrowser.DocumentText = _
        "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">" & vbCrLf & _
        "<html xmlns=""http://www.w3.org/1999/xhtml""><head><title>" & My.Application.Info.ProductName & "</title></head>" & vbCrLf & _
        "<frameset cols=""200px,*"" rows=""100%"" frameborder=""1"">" & vbCrLf & _
        "<frame src=""" & My.Application.Info.DirectoryPath & "\Help\index.htm"" scrolling=""auto"" name=""index"">" & vbCrLf & _
        "<frame src=""" & My.Application.Info.DirectoryPath & "\Help\" & HelpPage & """ scrolling=""auto"" name=""content"">" & vbCrLf & _
        "<noframes><body>" & vbCrLf & _
        "Appears that the browser does not like HTML Frames. Try <a href=""index.htm"">this</a>." & vbCrLf & _
        "</body></noframes></frameset></html>"

        Me.Show()
        Me.BringToFront()
        Me.Focus()

    End Function

End Class