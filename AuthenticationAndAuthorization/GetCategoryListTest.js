import http from 'k6/http';
import { check } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 10 },  // 10 کاربران همزمان
        { duration: '2m', target: 200 }, // افزایش به 50 کاربران
        { duration: '1m', target: 0 },  // کاهش به 0
    ],
};

export default function () {
    const res = http.get('http://localhost:7040/categories');
    check(res, {
        'response time < 200ms': (r) => r.timings.duration < 200,
    });
}
