import {defineStore} from "pinia";
import {reactive} from "vue";

export const NotificationTypes = Object.freeze({
    Info: 'NotifcationTypes/Info',
    Success: 'NotifcationTypes/Success',
});

export const useNotificationsStore = defineStore('notifications', () => {
    const notifications = reactive([]);

    function addNotification({ type = NotificationTypes.Info, message }) {
        notifications.push({ type, message });
    }
    
    return { 
        notifications,
        addNotification,
    };
});
