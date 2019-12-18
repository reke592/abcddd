import socket
import re
import sys

def print_summary(creates, inserts, selects, updates):
    print("create: %d" % creates)
    print("insert: %d" % inserts)
    print("select: %d" % selects)
    print("update: %d" % updates)

if __name__ == "__main__":
    creates = 0
    inserts = 0
    selects = 0
    updates = 0

    SERV = ('', 4000)
    backlog = 1
    szbuff = 1024
    q_create = re.compile(r"CREATE")
    q_insert = re.compile(r"INSERT")
    q_select = re.compile(r"SELECT")
    q_update = re.compile(r"UPDATE")

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind(SERV)
    server.listen(backlog)
    print("server listening on %s:%d" % SERV)
    while True:
        print("----------------------------")
        conn, addr = server.accept()
        while True:
            try:
                data = conn.recv(szbuff)
                if not data:
                    break
                if data == b'reset':
                    creates = 0
                    inserts = 0
                    updates = 0
                    selects = 0
                else:
                    q = data.decode('utf_8')
                    if q_create.match(q):
                        creates = creates + 1
                    elif q_insert.match(q):
                        inserts = inserts + 1
                    elif q_select.match(q):
                        selects = selects + 1
                    elif q_update.match(q):
                        updates = updates + 1
                    print("\rhits: %d" % (creates + inserts + selects + updates), end="")
                    sys.stdout.flush()
                    conn.send(b'OK')
            except Exception as e:
                print("\nsummary")
                print_summary(creates, inserts, selects, updates)
                break
