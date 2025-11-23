import { toast } from "react-toastify";

export const toastNotify = (id: any, res: any) => {
    
    if (id > 0) {
       
        switch (res === 'Success') {
            case false:
                if (res === null) {
                    toast.warning(res.data.message)
                }
                else {
                    toast.error(res.data.message)
                }
                break;
            case true:
                toast.success(res)
                break;
            default:
                toast.error('Something went wrong');
                break;
        }
    }

    toast.update(id, {
        render: res,
        type: res === "Success" ? "success" : "error",
        isLoading: false,
        autoClose: 3000,
    });
}