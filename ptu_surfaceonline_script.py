import requests
import threading
import datetime


#登陆
def login():
    s = requests.Session()
    #模拟头部信息
    headersp = {
            'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8',
            'User-Agent': 'Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36',
        }
    u = '你的学号'
    p = '你的密码'    
    url_login = "http://192.168.116.8/drcom/login?callback=dr1567554170962&DDDDD=%s&upass=%s&0MKKey=123456&R1=0&R3=0&R6=0&para=00&v6ip=&_=1567554113391" % (u,p) 
    fs = s.get(url_login, headers=headersp, allow_redirects=False)
    return fs.text
    
def valid():
    s = requests.Session()
    #模拟头部信息
    headersp = {
            'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8',
            'User-Agent': 'Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36',
        }
    url_login = "http://192.168.116.8/usermsg?url=1" 
    try :
        fs = s.get(url_login, headers=headersp, allow_redirects=False)
        return fs.text
    except :
        return 'no'

def checkbaidu():
    s = requests.Session()
    #模拟头部信息
    headersp = {
            'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8',
            'User-Agent': 'Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36',
        }
    url_login = "https://www.baidu.com" 
    try :
        fs = s.get(url_login, headers=headersp, allow_redirects=False)
        return fs.text
    except :
        return 'no'


def fun_timer():
    bd = checkbaidu()
    rs = bd.find('baidu',0)
    if rs > 0:
        data = valid();
        v = data.find('电脑端确认连接',0)
        if(v>0):
            print('连接正常,Time=',datetime.datetime.now())
        else:
            login()
            print('注销一次,消除检测,Time=',datetime.datetime.now())
            print()
    else:
        bd = login()
        mstr = '改成你的名字中的任意一个字'
        if bd.find('%s' % (mstr),0)<1:
            print('重新连接,Time=',datetime.datetime.now())
        else:
            print("重连成功,Time=",datetime.datetime.now())    
    global timer
    timer = threading.Timer(3,fun_timer)
    timer.start()

timer = threading.Timer(0,fun_timer)  #首次启动
timer.start()    