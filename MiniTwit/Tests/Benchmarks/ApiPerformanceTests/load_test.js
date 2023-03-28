import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '2m', target: 50 },
        { duration: '5m', target: 50 },
        { duration: '5m', target: 0 }
    ],
    thresholds: {
        http_req_duration: ['p(99)<500'],
    }
};

export default () => {
    http.batch([
        ['GET', 'http://souffle.nu:5000/api/Message'],
    ]);
    sleep(1);
};
