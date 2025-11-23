const PaymentModuleText = (props: any) => {
    const { paymentModule } = props;

    return (
        <div>
            <div title={paymentModule}
                className={`badge fs-7 badge-light-${paymentModule == 4 ? 'secondary' : 'primary'}`}>
                {paymentModule == 4 ? 'Facilitation Payment' : 'Deductible Payment'}
            </div>
        </div>
    )
}

export default PaymentModuleText
