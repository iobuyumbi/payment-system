import { PageLink } from "../_metronic/layout/core";

export const breadcrumbs=(props:any)=>{
    const profileBreadCrumbs: Array<PageLink> = [
        {
            title: props.title,
            path: props.path,
            isSeparator: false,
            isActive: false,
        },
    ]
return profileBreadCrumbs;   
}