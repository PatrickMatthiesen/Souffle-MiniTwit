--- minitwit.py	(original)
+++ minitwit.py	(refactored)
@@ -8,7 +8,7 @@
     :copyright: (c) 2010 by Armin Ronacher.
     :license: BSD, see LICENSE for more details.
 """
-from __future__ import with_statement
+
 import re
 import time
 import sqlite3
@@ -94,7 +94,7 @@
     redirect to the public timeline.  This timeline shows the user's
     messages as well as all the messages of followed users.
     """
-    print "We got a visitor from: " + str(request.remote_addr)
+    print("We got a visitor from: " + str(request.remote_addr))
     if not g.user:
         return redirect(url_for('public_timeline'))
     offset = request.args.get('offset', type=int)
