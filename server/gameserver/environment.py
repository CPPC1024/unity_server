import atexit
import io
import json
import os
import socket
import thread
import traceback
import struct
import time
from threading import Thread

from exception import UnityEnvironmentException, UnityActionException, UnityTimeOutException
from user import user


class UnityEnvironment(object):
    def __init__(self, base_port=5006):
        atexit.register(self.close)
        self.port = base_port
        self._buffer_size = 12000
        self._python_api = "API-2"
        self._loaded = False
        self._open_socket = False
        self.list_al = []
        self.list_con=[]
        self.conttt=0
        self.path="user.txt"


        print "socket port is:" + str(self.port) + " api: " + str(self._python_api)

        try:
            # Establish communication socket
            self._socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self._socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            self._socket.bind(("localhost", self.port))
            self._open_socket = True
        except socket.error:
            self._open_socket = True
            self.close()
            raise socket.error("Couldn't launch new environment because worker number is still in use. ")

        self._socket.settimeout(1000)
        try:
            self._socket.listen(1)
            while(1):
                try:

                    # _conn, _ = self._socket.accept()
                    _conn,_ = self._socket.accept()
                    _conn.settimeout(1000)
                    p = _conn.recv(self._buffer_size).decode('utf-8')
                    p = json.loads(p)
                except socket.timeout as e:
                    raise UnityTimeOutException(
                        "The Unity environment took too long to respond. Make sure does not need user interaction to ")

                self._unity_api = p["apiNumber"]
                self._academy_name = p["AcademyName"]
                self._log_path = p["logPath"]
                self._brain_names = p["brainNames"]
                self._external_brain_names = p["externalBrainNames"]
                print self._log_path
                print self._unity_api
                print self._external_brain_names
                print self._brain_names
                self._loaded = True

                thread.start_new_thread(self._recv_bytes,(_conn,))
                self.conttt += 1
        except UnityEnvironmentException:
            print("The Unity environment exception")
            self.close()
            raise

    def _recv_bytes(self,_conn):
        try:
            # self.conttt+=1
            # print(self.conttt)
            # s=""
            # if _conn:
            s = _conn.recv(self._buffer_size)
            print("s" + s)
            if s == b"":
                print("exit000")
                _conn.send(b"EXIT")
                _conn.close()
                # return 0

            if s != b"":
                message_length = struct.unpack("I", bytearray(s[:4]))[0]
                s = s[4:]
                while len(s) != message_length:
                    s += _conn.recv(self._buffer_size)

                print("rcv: " + s)
                # ss = s.split(",")
                if s == "close":
                    self.close( _conn)
                elif s == "send":#get user date

                    f = open("user.txt")
                    line = f.read()
                    self.list_al=json.loads(line.rstrip())
                    if len(self.list_al) > 0:
                        # ppp = False
                        # for u in self.list_al:
                        #     if u['name'] == ss[1]:
                        #         self._send_action(u, _conn)
                        #         ppp = True
                        #         break
                        # if ppp is not True:
                        #     self._send_action("0", _conn)
                        # for us in self.list_al:
                        #     self._send_action(us, _conn)
                        self._send_action(self.list_al, _conn)
                        self._recv_bytes(_conn)
                        # return 0
                    else:

                        self._send_action(self.list_al, _conn)
                        self._recv_bytes(_conn)
                        # return 0


                else:
                    uu = json.loads(s)

                    # f = open("user.txt")
                    # line = f.read()
                    # self.list_al = json.loads(line.rstrip())
                    print("ii")
                    print(self.list_al)
                    print("uu")
                    print(uu)
                    fl_s = False
                    for u in self.list_al:
                        for u0 in uu:
                            if u0['name'] is not None:
                                if u0['name'] == u['name']:
                                    fl_s = True
                                    break
                        if(fl_s == True):
                            break

                    if fl_s is False:
                        uu[0]['id'] = len(self.list_al) + 1
                        self.list_al.append(uu[0])
                        str = json.dumps(self.list_al)
                        with open("user.txt",'w') as f:
                            f.write(str)
                        self._send_action(self.list_al, _conn)
                    # self._send_action('suss', _conn)

                    if fl_s is not False:
                        # self._send_action(self.list_al, _conn)
                        # for i in self.list_al:
                        #     self.list_al.remove(i)

                        self.list_al=[]

                        for u0 in uu:
                            self.list_al.append(u0)

                        st = json.dumps(self.list_al)
                        with open("user.txt", 'w') as f:
                            f.write(st)
                        self._send_action(self.list_al, _conn)

                    self._recv_bytes(_conn)



            else:
                print("exit000")
                # _conn.send(b"EXIT")
                _conn.close()

            return 0
            # self._recv_bytes(_conn)
        except socket.timeout as e:
            # raise UnityTimeOutException("The environment took too long to respond.", self._log_path)
            print("timeout, will close socket")
            self.close(_conn)

    def _send_action(self, uu,_conn):
        try:
            print("send action")
            # action_message = {"name": name, "pass": ps, "hp": hp, "erp": erp, "bullet": bullet, "lp": lp}
            action_message = json.dumps(uu)
            _conn.send(action_message.encode('utf-8'))
        except UnityEnvironmentException:
            raise

    def close(self,_conn):
        print "env req close with arg, _loaded:" + str(self._loaded) + " _open_socket:" + str(self._open_socket)
        # traceback.print_stack()
        if self._loaded & self._open_socket:
            print("exit000")
            _conn.send(b"EXIT")
            _conn.close( _conn)
        if self._open_socket:
            print("exit1111")
            self._socket.close()
            self._loaded = False
        else:
            raise UnityEnvironmentException("No Unity environment is loaded.")
