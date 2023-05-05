import {defineStore} from "pinia";
import {reactive} from "vue";
import { v4 as uuid } from 'uuid';

export const NotificationTypes = Object.freeze({
    Info: 'NotifcationTypes/Info',
    Success: 'NotifcationTypes/Success',
});

export const useNotificationsStore = defineStore('notifications', () => {
    const notifications = reactive([]);

    function addNotification({ type = NotificationTypes.Info, message }) {
        const id = uuid();
        
        notifications.push({ type, message, id });
        
        setTimeout(() => {
            dismissNotification(id);
        }, 4000);
    }
    
    function dismissNotification(id) {
        const index = notifications.findIndex(n => n.id == id);
        
        if (index >= 0) {
            notifications.splice(index, 1);
        }
    }
    
    return { 
        notifications,
        addNotification,
        dismissNotification,
    };
});
