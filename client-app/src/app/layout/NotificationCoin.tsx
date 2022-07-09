import { toast } from "react-toastify";
import { NotificationCustom } from "../models/notificationCustom";
import { useStore } from "../stores/store";

export default function NotificationCoin(){
  const {notificationStore: {setNotification,  selectedNotification}} = useStore();
  setNotification();
  console.log(selectedNotification);

  if  (selectedNotification == null || undefined) return;

  toast(`💰 ${selectedNotification.message}`, {
    position: "top-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    })

  return (    <div>
    <h2>hello world</h2>
  </div>)
  }
