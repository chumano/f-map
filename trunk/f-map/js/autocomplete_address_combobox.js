function createAutoCompleteAddressCombobox(width, top, right, bottom, left, emptyText) {
    combobox = new Ext.form.ComboBox({
        width: width,
        /*typeAhead: true,*/
        margins: {
            top: top,
            right: right,
            bottom: bottom,
            left: left
        },
        emptyText: emptyText,
        hideTrigger: true, //use to hide nut so xuong
        triggerAction: 'all',
        // lastQuery: '',
        autoSelect: false,
        lazyRender: true,
        mode: 'local',
        // valueNotFoundText: 'Không có địa chỉ này',
        valueField: 'id',
        displayField: 'address',
        listeners: {
            'select': {
                fn: function (combo, value) {
                    // alert(value.data.id);
                }
            }
        }
    });

    sonha = '';
    combobox.on('beforequery', function (q) {
        index = 0;
        query = q.query;
        for (; index < query.length; ++index) {
            if (query.charAt(index) == ' ') {
                break;
            }
        }

        newQuery = '';
        if (index > 0) {
            newQuery = query.substring(index + 1, this.getRawValue().length);
            sonha = query.substring(0, index);
        } else {
            sonha = '';
            newQuery = query;
        }

        if (newQuery != '') {
            q.query = newQuery;
            return true;
        } else {
            return false;
        }
    });

    combobox.on('beforeselect', function (combox, record, index) {
        record.data.address = sonha + ' ' + record.data.address;
        return true;
    });

    combobox.on('blur', function (combo) {
        if (combo.getValue() == 0) {
            combo.clearValue();
            return false;
        }
        return true;
    });

    // combobox.setValue(0);

    return combobox;
}