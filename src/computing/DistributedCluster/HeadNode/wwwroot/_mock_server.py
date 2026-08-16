# -*- coding: utf-8 -*-
import SimpleHTTPServer, SocketServer, json, os
from datetime import datetime

ROOT = os.path.dirname(os.path.abspath(__file__))
PORT = 8765


class H(SimpleHTTPServer.SimpleHTTPRequestHandler):
    def __init__(self, *a, **k):
        SimpleHTTPServer.SimpleHTTPRequestHandler.__init__(self, *a, directory=ROOT, **k)

    def do_GET(self):
        if self.path.startswith('/api/status'):
            now = datetime.utcnow()
            ticks = (now - datetime(1, 1, 1)).total_seconds() * 1e7 + 621355968000000000
            nodes = []
            specs = [('node-a', '192.168.1.10', 16, 32768, 35, 48),
                     ('node-b', '192.168.1.11', 32, 65536, 72, 61),
                     ('node-c', '192.168.1.12', 8, 16384, 12, 30),
                     ('node-d', '192.168.1.13', 64, 131072, 88, 77)]
            for i, (nm, ip, cores, mem, cpu, memu) in enumerate(specs):
                nodes.append({
                    'nodeId': 'n' + str(i), 'online': True, 'machineName': nm,
                    'ipAddress': ip, 'cores': cores, 'cpuUsage': cpu,
                    'totalMemoryMB': mem, 'memoryUsage': memu,
                    'netUploadRate': 12.5 + i, 'netDownloadRate': 8.3 + i,
                    'currentBlock': 'block_' + str(i) if i % 2 == 0 else ''
                })
            totalCores = sum(n['cores'] for n in nodes)
            totalMem = sum(n['totalMemoryMB'] for n in nodes)
            cpuScore = float(totalCores) / 64.0
            memScore = (totalMem / 1024.0) / 256.0
            powerIndex = int(round((cpuScore * memScore) ** 0.5 * 100))
            status = {
                'clusterName': 'Darwinism Test', 'smbRoot': '\\\\smb\\share',
                'httpPort': 8080, 'pollInterval': 1500, 'totalJobs': 12,
                'pendingBlocks': 2, 'completedBlocks': 8, 'failedBlocks': 1,
                'runningBlocks': 3, 'onlineNodes': len(nodes), 'totalCores': totalCores,
                'totalMemoryMB': totalMem, 'powerIndex': powerIndex,
                'nodes': nodes, 'failures': [], 'logs': ['log line 1', 'log line 2'],
                'serverTime': int(ticks)
            }
            body = json.dumps(status).encode('utf-8')
            self.send_response(200)
            self.send_header('Content-Type', 'application/json')
            self.send_header('Content-Length', str(len(body)))
            self.end_headers()
            self.wfile.write(body)
            return
        SimpleHTTPServer.SimpleHTTPRequestHandler.do_GET(self)


httpd = SocketServer.TCPServer(('127.0.0.1', PORT), H)
print('serving on http://127.0.0.1:%d' % PORT)
httpd.serve_forever()
