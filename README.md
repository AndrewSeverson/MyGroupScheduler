MyGroupScheduler
================

An mvc 4 project that allows for the creation of users and groups to track events and news for those groups

** Ihis project is NOT complete yet. 
** If you have any questions please email me at AndrewJSeverson@gmail.com

Things that still need implementaion
 * Logger // i know i know
 * Sql injection checks
 * Prevent unlimited login attempts
 * Prevent spamming of any database insert method
 * Major Feature - the ability for admins to add/edit events
 * Major Feature - Displaying all events on the calendars
Once these are completed, the applicatoin will be functional to be published to the site

There is a lot of room for additions in the future:
 * Messaging system with signalr
 * Friends list
 * Commenting on events
 * Different types of events instead of one generic type
 * Notification system
 * I will add to this as i think of more

I am ignoring the web config for now until i have time to figure out a template for my passwords
Here is the config without the database passwords:

<?xml version="1.0" encoding="utf-8"?>
<!--
  For more information on how to configure your ASP.NET application, please visit
  http://go.microsoft.com/fwlink/?LinkId=169433
  -->
<configuration>
  <connectionStrings>
    <add name="ApplicationServices" connectionString="data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true" providerName="System.Data.SqlClient"/>
  </connectionStrings>
  <appSettings>
    <add key="webpages:Version" value="2.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="PreserveLoginUrl" value="true" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    <add key="DatabaseServerName" value="my01.winhost.com" />
    <add key="DatabaseName" value="mysql_60747_scheduler" />
    <add key="DatabaseUsername" value="*********" />
    <add key="DatabasePassword" value="*********" />
  </appSettings>
  <system.web>
    <trust level="Full" />
    <httpRuntime targetFramework="4.5" />
    <compilation debug="true" targetFramework="4.5" />
    <pages>
      <namespaces>
        <add namespace="System.Web.Helpers" />
        <add namespace="System.Web.Mvc" />
        <add namespace="System.Web.Mvc.Ajax" />
        <add namespace="System.Web.Mvc.Html" />
        <add namespace="System.Web.Routing" />
        <add namespace="System.Web.WebPages" />
      </namespaces>
    </pages>
    <httpModules>
      <!-- This section is used for IIS6 -->
      <add name="ContainerDisposal" type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web" />
      <add name="PropertyInjection" type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web" />
      <add name="AttributeInjection" type="Autofac.Integration.Web.Forms.AttributedInjectionModule, Autofac.Integration.Web" />
    </httpModules>
  </system.web>
  <system.webServer>
    <validation validateIntegratedModeConfiguration="false" />
    <!-- This section is used for IIS7 -->
    <modules>
      <add name="ContainerDisposal" type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web" preCondition="managedHandler" />
      <add name="PropertyInjection" type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web" preCondition="managedHandler" />
      <add name="AttributedInjection" type="Autofac.Integration.Web.Forms.AttributedInjectionModule, Autofac.Integration.Web" preCondition="managedHandler" />
    </modules>
  </system.webServer>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="4.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Web.WebPages" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.0.0.0" newVersion="2.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Autofac" publicKeyToken="17863af14b0044da" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-3.0.0.0" newVersion="3.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
