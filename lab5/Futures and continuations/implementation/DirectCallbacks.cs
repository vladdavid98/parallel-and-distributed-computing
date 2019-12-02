using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Futures_and_continuations.model;
using Futures_and_continuations.util;

namespace Futures_and_continuations.implementation {
    public static class DirectCallbacks {
        private static List<string> _hosts;

        public static void Run(List<string> hostnames) {
            _hosts = hostnames;

            for (var i = 0; i < _hosts.Count; i++) {
                DoStart(i);
                Thread.Sleep(5000);
            }
        }

        private static void DoStart(object idObject) {
            var id = (int) idObject;

            StartClient(_hosts[id], id);
        }

        /**
         * Connect to a server.
         */
        private static void StartClient(string host, int id) {
            // establish the remote endpoint of the server
            var ipHostInfo = Dns.GetHostEntry(host.Split('/')[0]);
            var ipAddress = ipHostInfo.AddressList[0];
            var remoteEndpoint = new IPEndPoint(ipAddress, HttpUtils.HttpPort);

            // create the TCP/IP socket
            var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // create a wrapper for the connection information
            var state = new StateObject {
                socket = client,
                hostname = host.Split('/')[0],
                endpoint = host.Contains("/") ? host.Substring(host.IndexOf("/")) : "/",
                remoteEndpoint = remoteEndpoint,
                id = id
            };
            
            // connect to the remote endpoint  
            state.socket.BeginConnect(state.remoteEndpoint, Connected, state);
        }

        private static void Connected(IAsyncResult ar) {
            // retrieve the details from the connection information wrapper
            var state = (StateObject) ar.AsyncState;
            var clientSocket = state.socket;
            var clientId = state.id;
            var hostname = state.hostname;

            // complete the connection  
            clientSocket.EndConnect(ar);
            Console.WriteLine("{0} --> Socket connected to {1} ({2})", clientId, hostname, clientSocket.RemoteEndPoint);

            // convert the string data to byte data using ASCII encoding.  
            var byteData = Encoding.ASCII.GetBytes(HttpUtils.GetRequestString(state.hostname, state.endpoint));

            // begin sending the data to the server  
            state.socket.BeginSend(byteData, 0, byteData.Length, 0, Sent, state);
        }

        private static void Sent(IAsyncResult ar) {
            var state = (StateObject) ar.AsyncState;
            var clientSocket = state.socket;
            var clientId = state.id;

            // complete sending the data to the server  
            var bytesSent = clientSocket.EndSend(ar);
            Console.WriteLine("{0} --> Sent {1} bytes to server.", clientId, bytesSent);

            // receive the response from the server
            // begin receiving the data from the server
            state.socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, Receiving, state);
        }

        private static void Receiving(IAsyncResult ar) {
            // retrieve the details from the connection information wrapper
            var state = (StateObject) ar.AsyncState;
            var clientSocket = state.socket;
            var clientId = state.id;

            try {
                // read data from the remote device.  
                var bytesRead = clientSocket.EndReceive(ar);

                // get from the buffer, a number of characters <= to the buffer size, and store it in the responseContent
                state.responseContent.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // if the response header has not been fully obtained, get the next chunk of data
                if (!HttpUtils.ResponseHeaderFullyObtained(state.responseContent.ToString())) {
                    clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, Receiving, state);
                } else {
                    // header has been fully obtained
                    // get the body
                    var responseBody = HttpUtils.GetResponseBody(state.responseContent.ToString());

                    // the custom header parser is being used to check if the data received so far has the length
                    // specified in the response headers
                    var contentLengthHeaderValue = HttpUtils.GetContentLength(state.responseContent.ToString());
                    if (responseBody.Length < contentLengthHeaderValue) {
                        // if it isn't, than more data is to be retrieve
                        clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, Receiving, state);
                    } else {
                        // otherwise, all the data has been received  
                        // write the response details to the console
                        foreach (var i in state.responseContent.ToString().Split('\r', '\n'))
                            Console.WriteLine(i);
                        Console.WriteLine(
                            "{0} --> Response received : expected {1} chars in body, got {2} chars (headers + body)",
                            clientId, contentLengthHeaderValue, state.responseContent.Length);

                        // release the socket
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
