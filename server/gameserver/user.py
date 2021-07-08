import atexit
import io
import json
import os
import socket
import traceback
import struct
import time


class user:
    def __init__(self):
        self.id = 0
        self.hp = 0
        self.erp = 0
        self.bullet = 0
        self.name = "0"
        self.pw = "0"
        self.lp = "0"
        self.x = 0
        self.y = 0
        self.z = 0


