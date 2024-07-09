import http.server
import socketserver
import os

class BrotliHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    def end_headers(self):
        if self.path.endswith(".br"):
            self.send_header("Content-Encoding", "br")
        super().end_headers()

    def do_GET(self):
        if self.path.endswith(".br"):
            self.path = self.path[:-3]
        super().do_GET()

PORT = 8080
DIRECTORY = "Build"

os.chdir(DIRECTORY)

Handler = BrotliHTTPRequestHandler
httpd = socketserver.TCPServer(("", PORT), Handler)

print("serving at port", PORT)
httpd.serve_forever()
