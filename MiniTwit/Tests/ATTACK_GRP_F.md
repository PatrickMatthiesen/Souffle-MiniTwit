# Penetration Test ZAP (Zed Attack Proxy)

# Vulnerabilities

## Risk: High
### 1. SQL Injection (MsSQL) might be possible:
The Zed attack, was able to use this in the Post query:
~~~
    username=ZAP%27+WAITFOR+DELAY+%270%3A0%3A15%27+--+&password=ZAP
~~~
The query time is controllable using parameter value [ZAP' WAITFOR DELAY '0:0:15' -- ], which caused the request to take [1.142.075] milliseconds, when the original unmodified query with value [ZAP] took [5.307] milliseconds.


### 2. SQL Injection (Oracle - Time Based) might be possible:
The Zed attack, was able to use this in the Post query:
~~~
    username=ZAP%27+%2F+%28SELECT++UTL_INADDR.get_host_name%28%2710.0.0.1%27%29+from+dual+union+SELECT++UTL_INADDR.get_host_name%28%2710.0.0.2%27%29+from+dual+union+SELECT++UTL_INADDR.get_host_name%28%2710.0.0.3%27%29+from+dual+union+SELECT++UTL_INADDR.get_host_name%28%2710.0.0.4%27%29+from+dual+union+SELECT++UTL_INADDR.get_host_name%28%2710.0.0.5%27%29+from+dual%29+%2F+%27&password=ZAP
~~~
which caused the request to take [12.051] milliseconds, when the original unmodified query with value [ZAP] took [6.048] milliseconds.

### Solution (SQL Injection):
    - Do not trust client side input, even if there is client side validation in place. 
    - In general, type check all data on the server side.
    - Apply an 'allow list' of allowed characters, or a 'deny list' of disallowed characters in user input.



## Risk: Medium
### Abscence of Anti-CSRF Tokens
The Zed attack found no Anti-CSRF tokens in a HTML submission form. A cross-site request forgery is an attack that involves forcing a victim to send an HTTP request to a target destination without their knowledge or intent in order to perform an action as the victim.
Present in:
~~~
    <div class=body>
        <h2>Sign Up</h2>
        <form action="/register" method=post>
            <dl>
            <dt>Username:
            <dd><input type=text name="username" size=30 value=""> 
            <dt>E-Mail:
            <dd><input type=text name="email" size=30 value="">
            <dt>Password:
            <dd><input type=password name="password" size=30>
            <dt>Password <small>(repeat)</small>:
            <dd><input type=password name="password2" size=30>
            </dl>
            <div class=actions><input type=submit value="Sign Up"></div>
        </form>
    </div>
~~~
The underlying cause is application functionality using predictable URL/form actions in a repeatable way: 
~~~
    <form action="/register" method=post>
~~~
The nature of the attack is that CSRF exploits the trust that a web site has for a user.

### Content Security Policy (CSP) Header Not Set
Content Security Policy (CSP) is an added layer of security that helps to detect and mitigate certain types of attacks, including Cross Site Scripting (XSS) and data injection attacks. These attacks are used for everything from data theft to site defacement or distribution of malware

### Solution (CSP): 
    Ensure that your web server, application server, load balancer, etc. is configured to set the     Content-Security-Policy header.

### Missing Anti-clickjacking Header
The response does not include either Content-Security-Policy with 'frame-ancestors' directive or X-Frame-Options to protect against 'ClickJacking' attacks.

### Solution (Anti-clickjacking)
    Modern Web browsers support the Content-Security-Policy and X-Frame-Options HTTP headers. Ensure one of them is set on all web pages returned by your site/app.

