import { toAbsoluteUrl } from "../_metronic/helpers";
import { Loader } from "../_shared/components";

export function NoDataFound(props: any) {
    const { colSpan, isLoading } = props;
    return (<div className=' mb-5 mb-xl-10'>
        <div className=' pt-9 pb-0'>
            <div className='d-flex flex-wrap flex-sm-nowrap mb-3'>
                <div className='flex-grow-1'>
                    <div className='d-flex justify-content-center flex-wrap mb-20' style={{ height: '225px' }}>
                        <div className="d-flex align-content-center flex-wrap">
                            <div className="d-flex flex-column bd-highlight mb-3">
                                <div className="p-2 bd-highlight"><img alt='No data' src={toAbsoluteUrl('media/no-data.png')} className='h-200px my-10 mb-10' /></div>
                                <div className="p-2 bd-highlight text-center"> <h3 className='fs-3 text-gray-600 fw-normal'>No data</h3>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

        // <div className='text-gray-600 fw-bold d-flex text-center w-50 align-content-center justify-content-center mx-20'>
        //     {isLoading ? "loading..." : "No matching records found"}
        // </div>

    )
}

export function LoadData(props: any) {
    return (
        <div className='text-gray-600 fw-bold d-flex text-center align-content-center justify-content-center mx-20'>
            {props.isLoading ? <Loader /> : "No matching records found"}
        </div>
    )
}
export function ServerMessage(props: any) {
    return (
        <div
            // className='text-danger fw-bold d-flex text-center w-50 align-content-center justify-content-center mx-20'
            style={{
                color: 'red',
                margin: '50px auto',
                height: '50px',
                width: '300px',
                textAlign: 'center',
                fontSize: '14px',

            }}
        >
            {props.message}
        </div>
    )
}
type LoadingStage = 'idle' | 'loading' | 'success' | 'error' | 'empty';

type Props = {
  stage: LoadingStage;
  children: React.ReactNode;
  errorMessage?: string;
};

export const ConditionalWrapper = ({ stage, children, errorMessage }: Props) => {
  if (stage === 'loading') {
    return <Loader />;
  }

  if (stage === 'error') {
    return <ServerMessage message={errorMessage || 'Something went wrong'} />;
  }

  if (stage === 'empty') {
    return <NoDataFound />;
  }

  return <>{children}</>;
};
