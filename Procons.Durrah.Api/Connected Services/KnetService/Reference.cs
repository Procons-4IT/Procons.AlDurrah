﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Procons.Durrah.Api.KnetService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="KnetService.IKnetService")]
    public interface IKnetService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKnetService/CallKnetGateway", ReplyAction="http://tempuri.org/IKnetService/CallKnetGatewayResponse")]
        Procons.Durrah.Common.Transaction CallKnetGateway(Procons.Durrah.Common.Transaction transaction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKnetService/CallKnetGateway", ReplyAction="http://tempuri.org/IKnetService/CallKnetGatewayResponse")]
        System.Threading.Tasks.Task<Procons.Durrah.Common.Transaction> CallKnetGatewayAsync(Procons.Durrah.Common.Transaction transaction);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKnetService/GetData", ReplyAction="http://tempuri.org/IKnetService/GetDataResponse")]
        string GetData();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKnetService/GetData", ReplyAction="http://tempuri.org/IKnetService/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKnetServiceChannel : Procons.Durrah.Api.KnetService.IKnetService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KnetServiceClient : System.ServiceModel.ClientBase<Procons.Durrah.Api.KnetService.IKnetService>, Procons.Durrah.Api.KnetService.IKnetService {
        
        public KnetServiceClient() {
        }
        
        public KnetServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KnetServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KnetServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KnetServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Procons.Durrah.Common.Transaction CallKnetGateway(Procons.Durrah.Common.Transaction transaction) {
            return base.Channel.CallKnetGateway(transaction);
        }
        
        public System.Threading.Tasks.Task<Procons.Durrah.Common.Transaction> CallKnetGatewayAsync(Procons.Durrah.Common.Transaction transaction) {
            return base.Channel.CallKnetGatewayAsync(transaction);
        }
        
        public string GetData() {
            return base.Channel.GetData();
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync() {
            return base.Channel.GetDataAsync();
        }
    }
}