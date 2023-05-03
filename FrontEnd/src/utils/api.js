import ky from "ky";

const instance = ky.extend({
    credentials: 'include',
    prefixUrl: 'https://localhost:7065/api',
});

export const api = instance;