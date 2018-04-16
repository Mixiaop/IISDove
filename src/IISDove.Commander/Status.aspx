<%@ Page Title="" Language="C#" MasterPageFile="~/Head.Master" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="IISDove.Commander.Status" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% var allSenders = IISDove.Current.CommanderCommand.GetAllSenders(); %>

    <div class="js-refresh" data-name="status">
        <div class="content">
            <div class="col-lg-12">
                <div class="underline-nav">
                    <nav class="underline-nav-body">
                        <a href="#" class="underline-nav-item selected" aria-selected="false" role="tab" title="Stars">所有鸽子</a>
                    </nav>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="content">
            <div class="row">
                <% if (allSenders != null)
                    { %>
                        <% foreach (var dove in allSenders)
    { %>
                <div class="col-lg-6">
                    <div class="block">
                        <div class="block-content table-responsive">
                            <span class="badge badge-success"><%= dove.Code %>_<%= dove.Name %></span> <span class="badge badge-info"><%= dove.Ip %></span> 最后检测用时 <span class="badge badge"><%= dove.LastCheckElapsedTime %> 秒</span>，最后接收于 <span class="badge"><%= dove.LastSendTime.ToDateTime().ToRelativeTime() %></span>
                            <table class="table table-hover js-dataTable-full">
                                    <thead>
                                        <tr>
                                            <th class="text-center" width="30%">SiteName</th>
                                            <th class="text-center" width="30%">Status</th>
                                            <th class="text-center">Localhost</th>
                                        </tr>
                                    </thead>
                                    <% if (dove.IISStatus != null)
                                        {
                                            foreach (var iis in dove.IISStatus)
                                            {%>
                                    <tr>
                                        <td class="text-center"><%= iis.SiteName %></td>
                                        <td class="text-center"><% if (iis.StatusCode == (int)IISDove.StatusCode.SiteOccurError)
                                                                    { %><i class="fa fa-circle text-danger"></i><%}
                                                                                                                         else
                                                                                                                         { %>
                                            <i class="fa fa-circle text-success"></i>
                                            <%} %></td>
                                        <td class="text-center"><%= iis.Localhost %></td>
                                    </tr>
                                    <%}

                                        } %>
                                </table>
                        </div>
                    </div>
                </div>
                <%}
    } %>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script>
        var vc = {};
        vc.options = {
            refreshTime: 2 * 1000
        };
        if (isNaN(vc.options.refreshTime)) {
            vc.options.refreshTime = 0;
        }

        vc.modules = {};

        vc.modules.refresh = function () {
            var _refreshData = function () {

                if (vc.options.refreshTime > 0) {
                    $.ajax(window.location.href, {
                        cache: false
                    }).done(function (html) {
                        var $res = $(html);
                        $('.js-refresh[data-name]').html($res.find('.js-refresh[data-name]').html());
                        if (vc.options.refreshTime > 0) {
                            setTimeout(_refreshData, vc.options.refreshTime);
                        }
                    })
                        .fail(function () {
                            console.log('Failed to refresh', this, arguments);
                        });
                }

            }

            return {
                init: function () { _refreshData(); }
            }
        }();


        vc.modules.refresh.init();


    </script>
</asp:Content>
