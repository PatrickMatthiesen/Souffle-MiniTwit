import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m', target: 30 },
        { duration: '5m', target: 30 },
        { duration: '2m', target: 60 },
        { duration: '5m', target: 60 },
        { duration: '2m', target: 90 },
        { duration: '5m', target: 90 },
        { duration: '10m', target: 0 }
    ]
};

export default () => {
    http.get('http://souffle.nu:5000/api/Message');
    sleep(1);
};
